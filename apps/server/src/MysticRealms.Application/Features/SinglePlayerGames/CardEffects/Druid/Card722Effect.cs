using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card722Effect : ICardEffect
{
    public void Execute(CardDto card, List<PartyMemberDto> partyMemberTargets, List<EnemyDto> enemyTargets, SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        // Add to board
        savedGame.Board.Add(new BoardCardRecord
        {
            Card = card,
            PartyMemberId = caster.PartyMemberId,
            EntryPoint = EntryPointType.AfterCardPlayed
        });

        // Emit BoardAdd action
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.BoardAdd,
            savedGame,
            cards: new List<CardDto> { card }
        ));
    }
}
