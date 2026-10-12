using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands;

public interface IBoardEffect
{
    void React(BoardCardRecord boardCard, SavedGameDto savedGame, List<GameAction> playActions);
}
