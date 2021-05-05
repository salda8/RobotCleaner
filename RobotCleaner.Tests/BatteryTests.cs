using RobotCleaner.Classes;
using Serilog;
using Xunit;
using Moq;
using RobotCleaner.Exceptions;

namespace RobotCleaner.Tests
{
    public class BatteryTests
    {
        private readonly Battery battery;

        public BatteryTests()
        {
            battery = new Battery(new Mock<ILogger>().Object) { Charge = 1 };
        }
        
        [Fact]
        public void IfThereIsNoBatteryChargeDecreaseChargeShouldThrowException()
        {
            Assert.Throws<NotEnoughBatteryException>(() => battery.DecreaseCharge(2));
        }
    }
}