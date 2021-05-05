using RobotCleaner.Models;

namespace RobotCleaner.Interfaces
{
    public interface IRobot
    {
        IInitializedRobot InitializeRobot(RobotInstruction robotInstruction);
    }
}