using System;
using System.Collections.Generic;
using FluentValidation;
using RobotCleaner.Exceptions;
using RobotCleaner.Interfaces;
using RobotCleaner.Models;
using RobotCleaner.Validators;
using Serilog;

namespace RobotCleaner.Classes
{
    public class Robot : IRobot, IInitializedRobot
    {
        private readonly ICompass compass;
        private readonly ILogger logger;
        private readonly IBattery battery;
        private readonly INavigation navigation;

        private readonly List<Location> cleanedLocations = new();

        private const int BackOffSequenceCapacity = 5;
        private Queue<IList<RobotCommand>> backOffSequencesQueue = new(BackOffSequenceCapacity);

        private readonly IList<IList<RobotCommand>> backOffSequences =
            new List<IList<RobotCommand>>(BackOffSequenceCapacity)
            {
                new List<RobotCommand>
                    { RobotCommand.TurnRight, RobotCommand.Advance, RobotCommand.TurnLeft },
                new List<RobotCommand>
                    { RobotCommand.TurnRight, RobotCommand.Advance, RobotCommand.TurnRight },
                new List<RobotCommand>
                    { RobotCommand.TurnRight, RobotCommand.Advance, RobotCommand.TurnRight },
                new List<RobotCommand>
                    { RobotCommand.TurnRight, RobotCommand.Back, RobotCommand.TurnRight, RobotCommand.Advance },
                new List<RobotCommand>
                    { RobotCommand.TurnLeft, RobotCommand.TurnLeft, RobotCommand.Advance }
            };


        public Robot(ILogger logger, ICompass compass, IBattery battery, INavigation navigation)
        {
            this.compass = compass;
            this.battery = battery;
            this.navigation = navigation;
            this.logger = logger;
            EnqueueBackoffSequence();
        }

        private void EnqueueBackoffSequence()
        {
            backOffSequencesQueue.Clear();

            foreach (IList<RobotCommand> backOffSequence in backOffSequences)
            {
                backOffSequencesQueue.Enqueue(backOffSequence);
            }
        }

        public IInitializedRobot InitializeRobot(RobotInstruction robotInstruction)
        {
            var validator = new RobotInstructionValidator();
            validator.ValidateAndThrow(robotInstruction);
            navigation.Map = robotInstruction.Map;
            navigation.CurrentLocation = robotInstruction.Start;
            battery.Charge = robotInstruction.Battery;

            return this;
        }

        public void ExecuteCommand(RobotCommand robotCommand)
        {
            Location navigationCurrentLocation = navigation.CurrentLocation;
            switch (robotCommand)
            {
                case RobotCommand.TurnLeft:
                    battery.DecreaseCharge(1);
                    navigationCurrentLocation.Facing =
                        compass.ExecuteCommand(navigationCurrentLocation.Facing, CompassCommand.TurnLeft);
                    break;
                case RobotCommand.TurnRight:
                    battery.DecreaseCharge(1);
                    navigationCurrentLocation.Facing =
                        compass.ExecuteCommand(navigationCurrentLocation.Facing, CompassCommand.TurnRight);
                    break;
                case RobotCommand.Advance:
                    battery.DecreaseCharge(2);
                    Move(MoveCommand.Advance);

                    break;
                case RobotCommand.Back:
                    battery.DecreaseCharge(3);
                    Move(MoveCommand.Back);

                    break;
                case RobotCommand.Clean:
                    battery.DecreaseCharge(5);
                    logger.Information(
                        $"cleaned: {navigationCurrentLocation}");
                    cleanedLocations.Add(new Location
                    {
                        X = navigationCurrentLocation.X,
                        Y = navigationCurrentLocation.Y,
                        Facing = navigationCurrentLocation.Facing
                    });

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(robotCommand), robotCommand, null);
            }
        }


        private void Move(MoveCommand command)
        {
            Location navigationCurrentLocation = navigation.CurrentLocation;
            switch (command)
            {
                case MoveCommand.Advance:

                    switch (navigationCurrentLocation.Facing)
                    {
                        case CardinalDirection.North:
                            TryToMove(navigationCurrentLocation.X - 1, navigationCurrentLocation.Y);

                            break;
                        case CardinalDirection.East:
                            TryToMove(navigationCurrentLocation.X, navigationCurrentLocation.Y + 1);

                            break;
                        case CardinalDirection.South:
                            TryToMove(navigationCurrentLocation.X + 1, navigationCurrentLocation.Y);

                            break;
                        case CardinalDirection.West:
                            TryToMove(navigationCurrentLocation.X, navigationCurrentLocation.Y - 1);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(command));
                    }

                    break;
                case MoveCommand.Back:
                    switch (navigationCurrentLocation.Facing)
                    {
                        case CardinalDirection.North:
                            TryToMove(navigationCurrentLocation.X + 1, navigationCurrentLocation.Y);

                            break;
                        case CardinalDirection.East:
                            TryToMove(navigationCurrentLocation.X, navigationCurrentLocation.Y - 1);

                            break;
                        case CardinalDirection.South:
                            TryToMove(navigationCurrentLocation.X - 1, navigationCurrentLocation.Y);

                            break;
                        case CardinalDirection.West:
                            TryToMove(navigationCurrentLocation.X, navigationCurrentLocation.Y + 1);

                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(command));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(command), command, null);
            }

            void TryToMove(int x, int y)
            {
                if (!navigation.TryToMoveToNextLocation(x, y))
                {
                    PerformBackOffStrategy();
                }
            }
        }

        private void PerformBackOffStrategy()
        {
            if (backOffSequencesQueue.Count == 0)
            {
                throw new RobotIsStuckException();
            }

            IList<RobotCommand> backOffSequence = backOffSequencesQueue.Dequeue();
            int backOffSequenceNumber = BackOffSequenceCapacity - backOffSequencesQueue.Count;
            logger.Warning(
                $"Performing Backoff sequence #{backOffSequenceNumber}. Commands in sequence: {string.Join(',', backOffSequence)}");
            foreach (RobotCommand command in backOffSequence)
            {
                logger.Information($"Performing backoff command: {command}");
                ExecuteCommand(command);
            }

            if (backOffSequenceNumber == 1)
            {
                logger.Warning(
                    "All Backoff sequences finished");
                EnqueueBackoffSequence();
            }
        }

        public CleaningResult CreateResult()
        {
            return new()
            {
                Visited = navigation.VisitedLocations,
                Cleaned = cleanedLocations,
                Final = new Location
                {
                    Facing = navigation.CurrentLocation.Facing,
                    X = navigation.CurrentLocation.X,
                    Y = navigation.CurrentLocation.Y
                },
                Battery = battery.Charge
            };
        }
    }
}