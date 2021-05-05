using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RobotCleaner.Models
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CardinalDirection
    {
        [EnumMember(Value = "N")]
        North,

        [EnumMember(Value = "E")]
        East,

        [EnumMember(Value = "S")]
        South,

        [EnumMember(Value = "W")]
        West
    }
}