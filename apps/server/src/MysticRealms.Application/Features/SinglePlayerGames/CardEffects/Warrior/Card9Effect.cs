using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card9Effect : ICardEffect
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

        caster.ApplyStrength(card.Strength);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Strength,
            savedGame,
            partyMembers: new[] { caster },
            enemies: Array.Empty<EnemyDto>()));

        caster.ApplyDefense(card.Defense);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Defense,
            savedGame,
            partyMembers: new[] { caster },
            enemies: Array.Empty<EnemyDto>()));
    }
}
