using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Features.SinglePlayerGames.EnemyActions;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Enums;
using MysticRealms.Domain.Games;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.EndTurn;

public class EndTurnCommandHandler(IGameRepository gameRepository) : IRequestHandler<EndTurnCommand, List<GameAction>>
{
    public async Task<List<GameAction>> Handle(EndTurnCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        var savedGame = SavedGameDto.FromJson(game!.Save!)!;

        
        savedGame.ActionLog = new List<GameAction>();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
            GameActionType.SetInitial,
            savedGame
        ));
        
        
        if (savedGame.Initiatives.Count == 0)
        {
            IncrementTurnAndPlan(savedGame);
        }

        savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));

        if (savedGame.Initiatives.Count == 0)
        {
            IncrementTurnAndPlan(savedGame);
            savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));
        }

        var gameOver = false;
        var playerTurnReady = false;

        while (savedGame.Initiatives.Count > 0 && !gameOver && !playerTurnReady)
        {
            var initiative = savedGame.Initiatives[0];

            if (initiative.Type == InitiativeType.Enemy)
            {
                var actingEnemy = savedGame.Enemies
                    .FirstOrDefault(e => e.InstanceId == initiative.EnemyInstanceId);

                if (actingEnemy is not null && actingEnemy.HitPoints > 0)
                {
                    bool enemyDiedToPoison = CombatTurnHelper.ConsumePoisonForEnemy(savedGame, actingEnemy.InstanceId);

                    if (enemyDiedToPoison)
                    {
                        savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));

                        CombatTurnHelper.AppendRoomWonIfApplicable(savedGame);

                        if (savedGame.Initiatives.Count == 0)
                        {
                            IncrementTurnAndPlan(savedGame);
                            savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));
                        }
                        continue;
                    }

                    // Clear armor entries sourced by this enemy (from its previous turn)
                    CombatTurnHelper.ClearArmorForEnemy(savedGame, actingEnemy.InstanceId);

                    var alivePartyMembersBefore = savedGame.PartyMembers
                        .Where(pm => pm.Health > 0)
                        .Select(pm => pm.PartyMemberId)
                        .ToList();

                    var enemyAction = EnemyActionFactory.GetAction(actingEnemy.Id);
                    var plan = EnemyActionPlanner.EnsurePlan(actingEnemy, savedGame);
                    enemyAction.Execute(actingEnemy, savedGame, plan);
                    actingEnemy.PlannedAction = null;

                    var newlyDeadPartyMembers = savedGame.PartyMembers
                        .Where(pm => pm.Health <= 0 && alivePartyMembersBefore.Contains(pm.PartyMemberId))
                        .ToList();

                    foreach (var deadMember in newlyDeadPartyMembers)
                    {
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                            GameActionType.Died,
                            savedGame,
                            partyMembers: new List<PartyMemberDto> { deadMember }));
                    }

                    if (savedGame.PartyMembers.All(pm => pm.Health <= 0))
                    {
                        CombatTurnHelper.MoveBoardCardsToDiscardPile(savedGame);
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.GameOver, savedGame));
                        gameOver = true;
                        game.Deactivate(GameStatus.Lost);
                    }
                }

                if (!gameOver)
                {
                    savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));

                    if (savedGame.Initiatives.Count == 0)
                    {
                        IncrementTurnAndPlan(savedGame);
                        savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));
                    }
                }
            }
            else if (initiative.Type == InitiativeType.Player)
            {
                var playerIndex = savedGame.Players.FindIndex(p => p.Id == initiative.PlayerInstanceId);
                if (playerIndex >= 0)
                {
                    var player = savedGame.Players[playerIndex];

                    bool playerWipedToPoison = CombatTurnHelper.ConsumePoisonForPlayer(savedGame, player.Id);

                    if (savedGame.PartyMembers.All(pm => pm.Health <= 0))
                    {
                        CombatTurnHelper.MoveBoardCardsToDiscardPile(savedGame);
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.GameOver, savedGame));
                        gameOver = true;
                        game.Deactivate(GameStatus.Lost);
                        break;
                    }

                    if (playerWipedToPoison)
                    {
                        savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));

                        if (savedGame.Initiatives.Count == 0)
                        {
                            IncrementTurnAndPlan(savedGame);
                            savedGame.Initiatives = CombatTurnHelper.GetInitiative(savedGame);
                            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));
                        }
                        continue;
                    }

                    // Clear armor entries sourced by this player's party members
                    CombatTurnHelper.ClearArmorForPlayer(savedGame, player.Id);

                    var aliveMembers = savedGame.PartyMembers
                        .Where(w => w.Health > 0 && w.PlayerId == player.Id)
                        .ToList();

                    var affectedAssassins = new List<PartyMemberDto>();
                    foreach (var member in aliveMembers)
                    {
                        if (member.ClassName == "Assassin")
                        {
                            member.Daggers = 3;
                            affectedAssassins.Add(member);
                        }
                    }

                    if (affectedAssassins.Count > 0)
                    {
                        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.Daggers, savedGame, partyMembers: affectedAssassins, enemies: new List<EnemyDto>()));
                    }

                    // Update mana to average of alive party members
                    var newMana = aliveMembers.Count > 0
                        ? (int)Math.Round(aliveMembers.Average(m => m.Mana))
                        : 0;

                    savedGame.Players[playerIndex].Mana = newMana;
                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateMana, savedGame));

                    // Discard hand and draw 5 new cards
                    CombatTurnHelper.DiscardAndDraw(savedGame, playerIndex, 5);

                    playerTurnReady = true;
                }
            }
        }

        game.SetSave(savedGame.ToJson());
        await gameRepository.UpdateAsync(game, cancellationToken);

        return savedGame.ActionLog;
    }

    private static void IncrementTurnAndPlan(SavedGameDto savedGame)
    {
        savedGame.Turn++;
        EnemyActionPlanner.PlanForTurn(savedGame);
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateTurn, savedGame, savedGame.Turn));
    }
}
