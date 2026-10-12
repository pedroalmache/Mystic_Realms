using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Warrior;

public class Card7Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var owner = savedGame.Players.FirstOrDefault(p => p.Id == card.PlayerId);
        if (owner is null)
            return;

        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        owner.Mana += card.Mana;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Mana,
            savedGame,
            partyMembers: caster is not null
                ? new List<PartyMemberDto> { caster }
                : new List<PartyMemberDto>()));
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateMana, savedGame));
    }
}
