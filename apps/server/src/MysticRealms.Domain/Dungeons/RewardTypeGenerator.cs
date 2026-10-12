namespace MysticRealms.Domain.Dungeons;

public class RewardTypeGenerator
{
    private static readonly RewardType[] BlessingRewardTypes =
    [
        RewardType.Health,
        RewardType.Offensive,
        RewardType.Defensive,
        RewardType.Mana,
        RewardType.Speed,
        RewardType.Gold,
        RewardType.Misc
    ];
    
    public RewardType Generate(RoomType roomType, Random rng)
    {
        if (roomType == RoomType.Encounter || roomType == RoomType.Treasure)
        {
            int rewardRoll = rng.Next(BlessingRewardTypes.Length);
            return BlessingRewardTypes[rewardRoll];
        }

        return RewardType.None;
    }
}
