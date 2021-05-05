using System.Collections.Generic;
using System.Linq;
using RobotCleaner.Exceptions;
using RobotCleaner.Interfaces;
using RobotCleaner.Models;
using Serilog;

namespace RobotCleaner.Classes
{
    public class Navigation : INavigation
    {
        private readonly ILogger logger;
        private const int LowerBound = 0;
        private static readonly IList<string> invalidLocations = new List<string> { "null", "C" };
        private int upperBound;
        private string[,] map;

        public string[,] Map
        {
            private get { return map; }
            set
            {
                map = value;
                upperBound = map.GetUpperBound(0);
            }
        }

        public List<Location> VisitedLocations { get; } = new();

        private Location currentLocation;

        public Location CurrentLocation
        {
            get => currentLocation;
            set
            {
                if (!IsLocationValid(value.X, value.Y))
                {
                    throw new InvalidStartLocationException();
                }

                currentLocation = value;
            }
        }

        public Navigation(ILogger logger)
        {
            this.logger = logger;
        }

        public bool TryToMoveToNextLocation(int x, int y)
        {
            if (!IsLocationValid(x, y))
            {
                return false;
            }

            CurrentLocation.X = x;
            CurrentLocation.Y = y;
            AddLocationToVisitedLocations(new Location()
                { X = CurrentLocation.X, Y = CurrentLocation.Y, Facing = CurrentLocation.Facing });

            return true;

            void AddLocationToVisitedLocations(Location visitedLocation)
            {
                logger.Information(
                    $"new visited location: {visitedLocation}");
                VisitedLocations.Add(visitedLocation);
            }
        }

        private bool IsLocationValid(int x, int y)
        {
            bool isOutOfBounds =
                new List<int> { x, y }.Any(locationIndex => locationIndex < LowerBound || locationIndex > upperBound);
            return !isOutOfBounds && invalidLocations.All(invalidLocation => invalidLocation != Map[x, y]);
        }
    }
}