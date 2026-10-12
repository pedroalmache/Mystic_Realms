using MysticRealms.Application.Features.SinglePlayerGames.Commands;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.CardEffects.Elementalist;

public class Card618Effect : ICardEffect
{
    public void Execute(
        CardDto card,
        List<PartyMemberDto> partyMemberTargets,
        List<EnemyDto> enemyTargets,
        SavedGameDto savedGame)
    {
        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        var casterList = caster is not null
            ? new List<PartyMemberDto> { caster }
            : new List<PartyMemberDto>();

        // Record Attack action
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.Attack,
            savedGame,
            partyMembers: casterList));

        var damage = card.MagicAttack + (caster?.Strength ?? 0);
        var aliveEnemies = savedGame.Enemies.Where(e => e.HitPoints > 0).ToList();

        if (aliveEnemies.Count == 0)
            return;

        // Base Magic Damage to all enemies
        foreach (var enemy in aliveEnemies)
        {
            enemy.ApplyMagicDamage(damage);
        }

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.MagicDamage,
            savedGame,
            partyMembers: new List<PartyMemberDto>(),
            enemies: aliveEnemies));

        // Synergy: Fire + Air
        if (caster is not null && caster.Fire && caster.Air)
        {
            // Extra Magic Damage to all enemies
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
            caster.Fire = false;
            caster.Air = false;
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Element,
                savedGame,
                partyMembers: casterList));
        }
    }
}
