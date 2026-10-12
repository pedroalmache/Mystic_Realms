using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;

public interface IEnemyAction
{
    void Execute(EnemyDto enemy, SavedGameDto savedGame);
}
