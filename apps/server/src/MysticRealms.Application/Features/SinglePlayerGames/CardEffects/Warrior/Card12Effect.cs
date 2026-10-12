using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card12Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        var damage = card.Attack + (caster?.Strength ?? 0);

        if (enemyTargets.Count != 1)
            throw new InvalidOperationException("Heavy Strike requires exactly one enemy target.");

        var targets = enemyTargets;

        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        foreach (var target in targets)
            target.ApplyPhysicalDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: targets));
    }
}
