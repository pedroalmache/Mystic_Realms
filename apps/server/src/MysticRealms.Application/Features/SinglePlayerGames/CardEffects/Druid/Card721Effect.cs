using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Druid;

public class Card721Effect : ICardEffect
{
    public void Execute(CardDto card, List<PartyMemberDto> partyMemberTargets, List<EnemyDto> enemyTargets, SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is null)
            return;

        switch (caster.Spirit)
        {
            case Spirit.Bear:
                caster.ApplyDefense(card.Defense);
                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                    GameActionType.Defense,
                    savedGame,
                    partyMembers: new List<PartyMemberDto> { caster }));
                break;

            case Spirit.Wolf:
                caster.ApplyStrength(card.Strength);
                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                    GameActionType.Strength,
                    savedGame,
                    partyMembers: new List<PartyMemberDto> { caster }));
                break;

            case Spirit.Deer:
                var playerIndex = savedGame.Players.FindIndex(p => p.Id == card.PlayerId);
                if (playerIndex != -1)
                {
                    CombatTurnHelper.DrawCardsForPlayer(savedGame, playerIndex, card.Draw);
                }
                break;

            case Spirit.Owl:
                var player = savedGame.Players.FirstOrDefault(p => p.Id == card.PlayerId);
                if (player is not null)
                {
                    player.Mana += card.Mana;
                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                        GameActionType.Mana,
                        savedGame,
                        partyMembers: new List<PartyMemberDto> { caster }));
                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateMana, savedGame));
                }
                break;
        }
    }
}
