using System.Text.Json.Serialization;

namespace MysticRealms.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EnemyIntentionType
{
    Attack,
    Defense,
    Special
}
