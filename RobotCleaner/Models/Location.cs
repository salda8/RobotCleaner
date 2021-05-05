namespace RobotCleaner.Models
{
    public class Location
    {
        public int X { get; set; }
        public int Y { get; set; }
        public CardinalDirection Facing { get; set; }

        public override string ToString()
        {
            return $"X:{X}, Y:{Y}, Facing:{Facing}";
        }
    }
}