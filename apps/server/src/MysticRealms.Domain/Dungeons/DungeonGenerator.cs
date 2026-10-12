using System.Security.Cryptography;
using System.Text;

namespace MysticRealms.Domain.Dungeons;

public class DungeonGenerator(
    RoomTypeGenerator roomTypeGenerator,
    RewardTypeGenerator rewardTypeGenerator,
    EnemyGenerator enemyGenerator
    )
{
    private static readonly RoomType[] CombatRoomTypes =
        [RoomType.Encounter, RoomType.Elite, RoomType.Boss];
    
    public Room GenerateRoom(string path, int level, int dungeonId)
    {
        string roomKey = path;

        int hash = GetStableHash(roomKey);
        var rng = new Random(hash);

        RoomType type = roomTypeGenerator.Generate(level, rng);
        RewardType reward = rewardTypeGenerator.Generate(type, rng);
        int[] enemyIds = CombatRoomTypes.Contains(type)
            ? enemyGenerator.Generate(level, rng)
            : [];
        int backgroundId = rng.Next(1, 7);

        return Room.Create(dungeonId, type, reward, roomKey, enemyIds, backgroundId);
    }

    public static int GetStableHash(string value)
    {
        byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return BitConverter.ToInt32(bytes, 0);
    }
}
