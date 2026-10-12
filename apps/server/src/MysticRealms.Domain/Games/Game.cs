using MysticRealms.Domain.Common;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Players;

namespace MysticRealms.Domain.Games;

public class Game : BaseEntity
{
    public new int Id { get; private set; }
    public string Code { get; private set; } = string.Empty;
    public string Save { get; private set; } = string.Empty;
    public bool IsMultiplayer { get; private set; } = false;
    public GameStatus GameStatus { get; private set; } = GameStatus.Open;
    public bool IsActive { get; private set; } = true;

    public Dungeon Dungeon { get; private set; }
    public ICollection<Player> Players { get; private set; } = new List<Player>();

    private Game() { }

    public static Game CreateSinglePlayer()
    {
        return new Game
        {
            Save = string.Empty,
            IsMultiplayer = false,
            IsActive = true
        };
    }

    public void SetSave(string save)
    {
        Save = save;
    }

    public void Deactivate(GameStatus gameStatus = GameStatus.Abandoned)
    {
        IsActive = false;
        UpdateGameStatus(gameStatus);
    }
    
    public void UpdateGameStatus(GameStatus gameStatus)
    {
        GameStatus = gameStatus;
    }
}