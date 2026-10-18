using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;

public interface IEnemyAction
{
    EnemyPlannedAction Plan(EnemyDto enemy, SavedGameDto savedGame, Random rng);
    void Execute(EnemyDto enemy, SavedGameDto savedGame, EnemyPlannedAction plan);
}
