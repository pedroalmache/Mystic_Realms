namespace MysticRealms.Domain.Dungeons;

public class EnemyGenerator
{
    private static readonly (int MaxLevel, int[][] Options)[] EncounterTiers =
    [
        // Levels 1-2: introductory enemies
        (2,
        [
            [1],
            [1, 1],
            [1, 2],
        ]),

        // Levels 3-5: moderate enemies
        (5,
        [
            [2],
            [2, 2],
            [1, 1, 2],
        ]),

        // Levels 6-9: tougher encounters
        (9,
        [
            [3],
            [2, 3],
            [2, 2, 3],
        ]),

        // Level 10+: elite enemies
        (int.MaxValue,
        [
            [4],
            [3, 4],
            [3, 3, 4],
        ]),
    ];

    public int[] Generate(int level, Random rng)
    {
        ArgumentNullException.ThrowIfNull(rng);
        if (level <= 0)
            throw new ArgumentOutOfRangeException(nameof(level), "Level must be greater than zero.");

        int[][] options = GetOptionsForLevel(level);
        return options[rng.Next(options.Length)];
    }

    private static int[][] GetOptionsForLevel(int level)
    {
        foreach (var (maxLevel, options) in EncounterTiers)
        {
            if (level <= maxLevel)
                return options;
        }
        return EncounterTiers[^1].Options;
    }
}
