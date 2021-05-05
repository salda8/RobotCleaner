using RobotCleaner.Models;

namespace RobotCleaner.Interfaces
{
    public interface IInitializedRobot
    {
        void ExecuteCommand(RobotCommand robotCommand);

        CleaningResult CreateResult();
    }
}