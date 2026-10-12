namespace MysticRealms.Domain.Dungeons;

public class Room
{
    public Guid Id { get; private set; }
    public int DungeonId { get; private set; }
    public RoomType Type { get; private set; }
    public RewardType Reward { get; private set; }
    public string Seed { get; private set; } = string.Empty;
    public int[] EnemyIds { get; private set; } = [];
    public int BackgroundId { get; private set; }

    public Dungeon Dungeon { get; private set; } = null!;

    private Room() { }

    public static Room Create(int dungeonId, RoomType type, RewardType reward, string seed, int[] enemyIds, int backgroundId)
    {
        return new Room
        {
            Id = Guid.NewGuid(),
            DungeonId = dungeonId,
            Type = type,
            Reward = reward,
            Seed = seed,
            EnemyIds = enemyIds,
            BackgroundId = backgroundId
        };
    }

    public void Update(RoomType type, RewardType reward, string seed, int[] enemyIds, int backgroundId)
    {
        Type = type;
        Reward = reward;
        Seed = seed;
        EnemyIds = enemyIds;
        BackgroundId = backgroundId;
    }
}
