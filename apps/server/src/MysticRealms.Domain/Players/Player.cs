using MysticRealms.Domain.Decks;
using MysticRealms.Domain.Games;
using MysticRealms.Domain.Identity;
using MysticRealms.Domain.PartyMembers;

namespace MysticRealms.Domain.Players;

public class Player
{
    public int Id { get; private set; }
    public bool IsHost { get; private set; }
    public int GameId { get; private set; }
    public string UserId { get; private set; }

    public ApplicationUser User { get; private set; } = null!;
    public Game Game { get; private set; } = null!;
    public ICollection<Deck> Deck { get; private set; } = null!;
    public ICollection<PartyMember> PartyMembers { get; private set; } = new List<PartyMember>();

    private Player() { }

    public static Player Create(int gameId, string userId, bool isHost = true)
    {
        return new Player
        {
            GameId = gameId,
            UserId = userId,
            IsHost = isHost
        };
    }
}