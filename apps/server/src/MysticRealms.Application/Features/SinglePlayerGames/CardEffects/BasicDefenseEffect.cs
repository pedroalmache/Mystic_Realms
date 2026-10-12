using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects;

public class BasicDefenseEffect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        if (caster is not null)
        {
            caster.ApplyArmor(card.Armor);

            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Armor,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }
    }
}
