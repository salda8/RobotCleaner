using System.Collections.Generic;
using RobotCleaner.Interfaces;
using RobotCleaner.Models;
using Serilog;

namespace RobotCleaner.Classes
{
    public class Compass : ICompass
    {
        private readonly ILogger logger;

        private readonly Dictionary<CardinalDirection, Dictionary<CompassCommand, CardinalDirection>> moves =
            new()
            {
                {
                    CardinalDirection.North,
                    new Dictionary<CompassCommand, CardinalDirection>
                    {
                        { CompassCommand.TurnLeft, CardinalDirection.West },
                        { CompassCommand.TurnRight, CardinalDirection.East }
                    }
                },
                {
                    CardinalDirection.East,
                    new Dictionary<CompassCommand, CardinalDirection>
                    {
                        { CompassCommand.TurnLeft, CardinalDirection.North },
                        { CompassCommand.TurnRight, CardinalDirection.South }
                    }
                },
                {
                    CardinalDirection.West,
                    new Dictionary<CompassCommand, CardinalDirection>
                    {
                        { CompassCommand.TurnLeft, CardinalDirection.South },
                        { CompassCommand.TurnRight, CardinalDirection.North }
                    }
                },
                {
                    CardinalDirection.South,
                    new Dictionary<CompassCommand, CardinalDirection>
                    {
                        { CompassCommand.TurnLeft, CardinalDirection.East },
                        { CompassCommand.TurnRight, CardinalDirection.West }
                    }
                }
            };

        public Compass(ILogger logger)
        {
            this.logger = logger;
        }

        public CardinalDirection ExecuteCommand(CardinalDirection initialCardinalDirection, CompassCommand command)
        {
            CardinalDirection cardinalDirection = moves[initialCardinalDirection][command];
            logger.Information($"Facing new direction: {cardinalDirection}");
            return cardinalDirection;
        }
    }
}