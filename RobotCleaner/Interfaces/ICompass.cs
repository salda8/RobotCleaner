using RobotCleaner.Models;

namespace RobotCleaner.Interfaces
{
    public interface ICompass
    {
        CardinalDirection ExecuteCommand(CardinalDirection initialCardinalDirection, CompassCommand command);
    }
}