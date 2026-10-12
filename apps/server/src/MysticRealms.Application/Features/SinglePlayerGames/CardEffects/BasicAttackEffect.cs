using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects;

public class BasicAttackEffect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        var damage = card.Attack + (caster?.Strength ?? 0);

        // Attack action: snapshot the attacking party member
        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        // Apply physical damage to each target (armor first, oldest to newest, then HP)
        foreach (var target in enemyTargets)
            target.ApplyPhysicalDamage(damage);

        // PhysicalDamage action: snapshot only the targeted enemies (no party members)
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));
    }
}