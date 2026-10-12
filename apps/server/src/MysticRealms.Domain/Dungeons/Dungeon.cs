using MysticRealms.Domain.Common;
using MysticRealms.Domain.Games;

namespace MysticRealms.Domain.Dungeons;

public class Dungeon : BaseEntity
{
    public int Id { get; private set; }
    public int GameId { get; private set; }
    public string Seed { get; private set; }
    public string Path { get; private set; }
    public int Level { get; private set; } = 0;
    
    public Game Game { get; private set; }

    private Dungeon(int gameId, string seed)
    {
        GameId = gameId;
        Seed = seed;
        Path = Seed;
    }

    public static Dungeon Create(int gameId, string seed)
    {
        return new Dungeon(gameId, seed);
    }

    public string NextRoom(string option)
    {
        Path += $@"-{option}";
        Level++;
        return Path;
    }
}