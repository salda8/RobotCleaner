using System.Collections.Generic;

namespace RobotCleaner.Models
{
    public class CleaningResult
    {
        public List<Location> Visited { get; set; }
        public List<Location> Cleaned { get; set; }
        public Location Final { get; set; }
        public int Battery { get; set; }
    }
}