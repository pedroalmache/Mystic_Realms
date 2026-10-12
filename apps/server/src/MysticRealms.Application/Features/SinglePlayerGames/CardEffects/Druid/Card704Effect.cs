using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card704Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);

        foreach (var target in enemyTargets)
        {
            if (caster is not null)
            {
                target.ApplyPoison(card.Poison, caster.Identifier);
            }
            else
            {
                target.ApplyPoison(card.Poison, Guid.Empty);
            }
        }

        // Snapshot only the targeted enemies
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Poison,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: enemyTargets));
    }
}
