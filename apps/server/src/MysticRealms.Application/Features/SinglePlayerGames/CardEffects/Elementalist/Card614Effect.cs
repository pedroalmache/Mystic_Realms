using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card614Effect : ICardEffect
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

        // Base effect: Draw cards
        var playerIndex = savedGame.Players.FindIndex(p => p.Id == caster.PlayerId);
        if (playerIndex != -1)
        {
            CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, card.Draw);
        }

        // Apply Air essence
        caster.Air = true;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Element,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));
    }
}
