using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;

public class EnemyPlannedAction
{
    public EnemyPlannedActionKind Kind { get; set; }
    public int Amount { get; set; }
    public Guid? TargetEnemyInstanceId { get; set; }
    public int? TargetPartyMemberId { get; set; }

    public List<EnemyIntentionType> Intentions => DeriveIntentions(Kind);

    public static List<EnemyIntentionType> DeriveIntentions(EnemyPlannedActionKind kind) =>
        kind switch
        {
            EnemyPlannedActionKind.PhysicalAttack or EnemyPlannedActionKind.MagicAttack
                => [EnemyIntentionType.Attack],
            EnemyPlannedActionKind.PhysicalArmor or EnemyPlannedActionKind.MagicArmor
                => [EnemyIntentionType.Defense],
            _ => [EnemyIntentionType.Special]
        };

    public static List<EnemyIntentionType> DeriveIntentions(IEnumerable<EnemyPlannedActionKind> kinds)
    {
        var set = new HashSet<EnemyIntentionType>();
        foreach (var kind in kinds)
        {
            foreach (var intention in DeriveIntentions(kind))
                set.Add(intention);
        }

        return
        [
            .. new[] { EnemyIntentionType.Attack, EnemyIntentionType.Defense, EnemyIntentionType.Special }
                .Where(set.Contains)
        ];
    }
}
