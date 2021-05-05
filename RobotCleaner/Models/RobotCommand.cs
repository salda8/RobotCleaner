using System.Runtime.Serialization;

namespace RobotCleaner.Models
{
    public enum RobotCommand
    {
        [EnumMember(Value = "TL")]
        TurnLeft,

        [EnumMember(Value = "TR")]
        TurnRight,

        [EnumMember(Value = "A")]
        Advance,

        [EnumMember(Value = "B")]
        Back,

        [EnumMember(Value = "C")]
        Clean
    }
}