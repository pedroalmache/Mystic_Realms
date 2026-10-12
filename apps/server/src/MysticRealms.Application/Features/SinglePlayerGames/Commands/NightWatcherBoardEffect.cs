using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands;

public class NightWatcherBoardEffect : IBoardEffect
{
    public void React(BoardCardRecord boardCard, SavedGameDto savedGame, List<GameAction> playActions)
    {
        var magicDamageActions = playActions.Where(a => a.Type == GameActionType.MagicDamage).ToList();
        if (magicDamageActions.Count == 0)
            return;

        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == boardCard.PartyMemberId);
        if (caster != null && caster.Health > 0 && caster.Spirit == Spirit.Owl)
        {
            var damage = boardCard.Card.MagicAttack;
            foreach (var action in magicDamageActions)
            {
                if (action.Enemies == null || action.Enemies.Count == 0) continue;

                var targetInstanceIds = action.Enemies.Select(e => e.InstanceId).ToHashSet();
                var targets = savedGame.Enemies.Where(e => targetInstanceIds.Contains(e.InstanceId) && e.HitPoints > 0).ToList();

                if (targets.Count > 0)
                {
                    foreach (var target in targets)
                    {
                        target.ApplyMagicDamage(damage);
                    }

                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                        GameActionType.MagicDamage,
                        savedGame,
                        partyMembers: new List<PartyMemberDto>(),
                        enemies: targets));
                }
            }
        }
    }
}
