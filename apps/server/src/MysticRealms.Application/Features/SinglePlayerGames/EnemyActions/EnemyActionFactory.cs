using MysticRealms.Application.Features.SinglePlayerGames.EnemyActions.Enemy1;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;

public static class EnemyActionFactory
{
    public static IEnemyAction GetAction(int enemyId)
    {
        return enemyId switch
        {
            1 => new Enemy1Action(),
            _ => new Enemy1Action()
        };
    }
}
