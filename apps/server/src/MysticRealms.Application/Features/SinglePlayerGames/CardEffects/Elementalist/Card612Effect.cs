using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card612Effect : ICardEffect
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

        // Base effect: Gain Armor
        caster.ApplyArmor(card.Armor);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Armor,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));

        // Synergy check: Water + Earth
        bool hasWater = caster.Water || card.Water == true;
        bool hasEarth = caster.Earth || card.Earth == true;

        if (hasWater && hasEarth)
        {
            caster.ApplyMagicArmor(card.MagicArmor);
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.MagicArmor,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));

            // Consume elements
            caster.Water = false;
            caster.Earth = false;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster }));
        }

        // Apply Earth essence
        caster.Earth = true;
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Element,
            savedGame,
            partyMembers: new List<PartyMemberDto> { caster }));
    }
}
