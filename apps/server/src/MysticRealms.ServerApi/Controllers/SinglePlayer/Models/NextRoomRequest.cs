using System.Text.Json.Serialization;

namespace MysticRealms.ServerApi.Controllers.SinglePlayer.Models;

public class NextRoomRequest
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NextRoomDirection Direction { get; set; }
}

public enum NextRoomDirection
{
    Left,
    Right,
    Forward
}
