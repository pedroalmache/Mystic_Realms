using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card711Effect : ICardEffect
{
    public void Execute(CardDto card, List<PartyMemberDto> partyMemberTargets, List<EnemyDto> enemyTargets, SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        var damage = card.Attack + (caster?.Strength ?? 0);
        var attackPartyMembers = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: attackPartyMembers));

        foreach (var target in enemyTargets)
            target.ApplyPhysicalDamage(damage);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.PhysicalDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));

        caster!.ApplySpirit((Spirit)card.Spirit!);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Spirit,
            savedGame,
            partyMembers: new List<PartyMemberDto>{caster},
            enemies: new List<EnemyDto>()));
    }
}
