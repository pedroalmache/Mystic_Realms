using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card617Effect : ICardEffect
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

        var casterList = new List<PartyMemberDto> { caster };

        // Base effect: Gain Magic Armor
        caster.ApplyMagicArmor(card.MagicArmor);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicArmor,
            savedGame,
            partyMembers: casterList));

        // Synergy: Earth + Air
        bool hasEarth = caster.Earth || (card.Earth ?? false);
        bool hasAir = caster.Air || (card.Air ?? false);

        if (hasEarth && hasAir)
        {
            // Deal 2x Damage.
            // Interpreted as 2x the Magic Armor value of the card as Magic Damage to all enemies.
            var damage = (card.MagicArmor * 2) + caster.Strength;

            // Log Attack animation
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Attack,
                savedGame,
                partyMembers: casterList));

            var aliveEnemies = savedGame.Enemies.Where(e => e.HitPoints > 0).ToList();
            foreach (var enemy in aliveEnemies)
            {
                enemy.ApplyMagicDamage(damage);
            }

            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.MagicDamage,
                savedGame,
                partyMembers: new List<PartyMemberDto>(),
                enemies: aliveEnemies));

            // Consume elements
            caster.Earth = false;
            caster.Air = false;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: casterList));
        }
    }
}
