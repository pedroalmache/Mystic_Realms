using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card8Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        if (enemyTargets.Count != 1)
            throw new InvalidOperationException("Crushing Guard requires exactly one enemy target.");

        var targets = enemyTargets;

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));

        var damage = caster.TotalArmor + caster.Strength;
        foreach (var target in targets)
            target.ApplyPhysicalDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: targets));
    }
}
