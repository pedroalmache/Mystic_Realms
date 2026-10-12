using MysticRealms.Domain.Dungeons;

namespace MysticRealms.Domain.Blessings;

public class Blessing
{
    public int Id { get; private set; }
    public RewardType Type { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Level_1_Description { get; private set; } = string.Empty;
    public string Level_2_Description { get; private set; } = string.Empty;
    public string Level_3_Description { get; private set; } = string.Empty;
    public string Level_4_Description { get; private set; } = string.Empty;

    private Blessing() { }
}
