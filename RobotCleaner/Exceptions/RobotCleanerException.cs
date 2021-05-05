using System;

namespace RobotCleaner.Exceptions
{
    public abstract class RobotCleanerException : Exception
    {
        protected RobotCleanerException(string message) : base(message)
        {
        }
    }
}