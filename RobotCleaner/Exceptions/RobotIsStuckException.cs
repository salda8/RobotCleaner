using System;

namespace RobotCleaner.Exceptions
{
    public class RobotIsStuckException : Exception
    {
        public RobotIsStuckException(string message = "Robot is stuck") : base(message)
        {
        }
    }
}