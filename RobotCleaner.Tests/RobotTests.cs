using Moq;
using RobotCleaner.Classes;
using RobotCleaner.Exceptions;
using RobotCleaner.Interfaces;
using RobotCleaner.Models;
using Serilog;
using Xunit;

namespace RobotCleaner.Tests
{
    public class RobotTests
    {
        private readonly Mock<INavigation> navigation = new();
        private readonly Mock<IBattery> battery = new();

        [Fact]
        private void RobotWhichCantMoveAnywhereShouldThrowRobotIsStuckException()
        {
            navigation.Setup(x => x.TryToMoveToNextLocation(It.IsAny<int>(), It.IsAny<int>())).Returns(false);
            navigation.Setup(x => x.CurrentLocation)
                .Returns(new Location() { X = 0, Y = 0, Facing = CardinalDirection.East });
            battery.Setup(x => x.DecreaseCharge(It.IsAny<int>()));
            var robot = new Robot(new Mock<ILogger>().Object, new Compass(new Mock<ILogger>().Object),
                battery.Object, navigation.Object);
            Assert.Throws<RobotIsStuckException>(() => robot.ExecuteCommand(RobotCommand.Advance));
        }
    }
}