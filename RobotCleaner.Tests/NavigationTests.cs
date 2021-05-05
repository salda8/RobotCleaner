using FluentAssertions;
using Moq;
using RobotCleaner.Classes;
using RobotCleaner.Exceptions;
using RobotCleaner.Models;
using Serilog;
using Xunit;

namespace RobotCleaner.Tests
{
    public class NavigationTests
    {
        private readonly string[,] map =
        {
            { "null", "C", "S", "S" },
            { "S", "S", "S", "S" },
            { "S", "S", "S", "S" },
            { "S", "S", "S", "S" }
        };

        private readonly Navigation navigation;

        public NavigationTests()
        {
            navigation = new Navigation(new Mock<ILogger>().Object) { Map = map };
        }

        [Fact]
        public void IfInitialLocationIsInvalidItShouldThrowException()
        {
            Assert.Throws<InvalidStartLocationException>(() =>
                navigation.CurrentLocation = new Location() { X = 0, Y = 0 });
        }

        [Fact]
        public void IfTryingToMoveToInvalidLocationItShouldReturnFalse()
        {
            navigation.TryToMoveToNextLocation(0, 1).Should().BeFalse();
        }
    }
}