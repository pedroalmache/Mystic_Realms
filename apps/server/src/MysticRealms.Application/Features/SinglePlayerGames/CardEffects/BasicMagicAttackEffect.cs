using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects;

public class BasicMagicAttackEffect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        // Attack action: snapshot the attacking party member
        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        var damage = card.MagicAttack + (caster?.Strength ?? 0);

        // Apply magic damage to each target
        foreach (var target in enemyTargets)
            target.ApplyMagicDamage(damage);

        // MagicDamage action: snapshot only the targeted enemies
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));
    }
}
