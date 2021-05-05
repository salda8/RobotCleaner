using System.Collections.Generic;

namespace RobotCleaner.Models
{
    public class RobotInstruction
    {
        public IList<RobotCommand> Commands { get; set; }
        public int Battery { get; set; }
        public Location Start { get; set; }
        public string[,] Map { get; set; }
    }
}