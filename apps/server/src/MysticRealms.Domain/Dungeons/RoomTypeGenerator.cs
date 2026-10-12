namespace MysticRealms.Domain.Dungeons;

/// <summary>
/// Determines the <see cref="RoomType"/> for a dungeon room based on the current
/// level and a seeded <see cref="Random"/> instance.
/// </summary>
public class RoomTypeGenerator
{
    /// <summary>
    /// Determines the room type for the given <paramref name="level"/> using
    /// <paramref name="rng"/> for any probabilistic decisions.
    /// </summary>
    public RoomType Generate(int level, Random rng)
    {
        // First 4 rooms are always Encounters
        if (level <= 4)
            return RoomType.Encounter;

        // Every 20th room is a Boss
        if (level % 20 == 0)
            return RoomType.Boss;

        // Every 5th room is an Elite
        if (level % 5 == 0)
            return RoomType.Elite;

        // Generate a random number between 0 and 99
        int roll = rng.Next(100);

        // 70% chance for Encounter
        if (roll < 70)
            return RoomType.Encounter;

        // 5% chance for each: Treasure, Gold, Merchant
        if (roll < 75)
            return RoomType.Treasure;
        if (roll < 80)
            return RoomType.Gold;
        if (roll < 85)
            return RoomType.Merchant;

        // 5% chance for Smith
        if (roll < 90)
            return RoomType.Smith;

        // 10% chance for Healing
        return RoomType.Healing;
    }
}
