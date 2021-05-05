namespace RobotCleaner.Exceptions
{
    public class NotEnoughBatteryException : RobotCleanerException
    {
        public NotEnoughBatteryException(string message = "Not enough battery") : base(message)
        {
        }
    }
}