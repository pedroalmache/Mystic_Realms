using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card608Effect : ICardEffect
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

        // Base effect: Gain Magic Armor
        caster.ApplyMagicArmor(card.MagicArmor);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicArmor,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));

        // Check for Water + Earth synergy.
        // Card 608 is Water, so Water is guaranteed active for this turn if we consider the card itself.
        // However, the rule typically checks for essences ALREADY active or being activated.
        // Based on other effects (like Card 606), the card sets the essence AFTER some logic,
        // but synergy checks usually include the current card's element.

        bool hasWater = caster.Water || card.Water == true;
        bool hasEarth = caster.Earth || card.Earth == true;

        if (hasWater && hasEarth)
        {
            caster.ApplyArmor(card.Armor);
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Armor,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }

        // Apply Water essence
        caster.Water = true;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Element,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));
    }
}
