using System.Collections.Generic;
using RobotCleaner.Models;

namespace RobotCleaner.Interfaces
{
    public interface INavigation
    {
        string[,] Map { set; }

        List<Location> VisitedLocations { get; }

        bool TryToMoveToNextLocation(int x, int y);

        Location CurrentLocation { get; set; }
    }
}