using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card713Effect : ICardEffect
{
    public void Execute(CardDto card, List<PartyMemberDto> partyMemberTargets, List<EnemyDto> enemyTargets, SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        caster.Speed += 2;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.Speed, savedGame, partyMembers: new[] { caster }));
        var playerIndex = savedGame.Players.FindIndex(p => p.Id == card.PlayerId);
        CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, card.Draw);

        caster!.ApplySpirit((Spirit)card.Spirit!);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Spirit,
            savedGame,
            partyMembers: new List<PartyMemberDto>{caster},
            enemies: new List<EnemyDto>()));
    }
}
