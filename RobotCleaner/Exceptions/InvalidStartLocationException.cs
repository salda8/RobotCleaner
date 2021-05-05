namespace RobotCleaner.Exceptions
{
    public class InvalidStartLocationException : RobotCleanerException
    {
        public InvalidStartLocationException(string message = "Invalid start location") : base(message)
        {
        }
    }
}