using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.CardEffects;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.PlayCard;

public class PlayCardCommandHandler(IGameRepository gameRepository, IRoomRepository roomRepository) : IRequestHandler<PlayCardCommand, List<GameAction>>
{
    public async Task<List<GameAction>> Handle(PlayCardCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        var savedGame = SavedGameDto.FromJson(game!.Save!)!;

        // Find the card instance in any player's hand
        CardDto? card = null;
        int playerIndex = -1;
        for (int playerIdx = 0; playerIdx < savedGame.Players.Count; playerIdx++)
        {
            card = savedGame.Players[playerIdx].Hand.FirstOrDefault(c => c.InstanceId == request.CardInstanceId);
            if (card is not null)
            {
                playerIndex = playerIdx;
                break;
            }
        }

        if (card is null)
            throw new InvalidOperationException("Card not found in any player's hand.");

        if (savedGame.Players[playerIndex].Mana < card.ManaCost)
            return new List<GameAction> { CombatTurnHelper.BuildAction(GameActionType.NotEnoughMana, savedGame) };

        var partyMemberTargets = savedGame.PartyMembers
            .Where(pm => request.PartyMemberInstanceIds.Contains(pm.Identifier) && pm.Health > 0)
            .ToList();

        var enemyTargets = savedGame.Enemies
            .Where(e => request.EnemyInstanceIds.Contains(e.InstanceId) && e.HitPoints > 0)
            .ToList();

        savedGame.ActionLog.Clear();
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.SetInitial, savedGame));

        // Deduct mana cost and emit UpdateMana action
        savedGame.Players[playerIndex].Mana -= card.ManaCost;
        card.UseCount++;
        savedGame.Players[playerIndex].Hand.Remove(card);

        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateMana, savedGame));
        
        // Build the CardPlayed action
        savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.CardPlayed, savedGame, cards: new List<CardDto> { card }));

        var deadEnemiesBeforeEffects = savedGame.Enemies
            .Where(e => e.HitPoints <= 0)
            .Select(e => e.InstanceId)
            .ToHashSet();

        var caster = savedGame.PartyMembers.FirstOrDefault(pm => pm.PartyMemberId == card.PartyMemberId);
        int actionLogCountBefore = savedGame.ActionLog.Count;
        int casterArmorBefore = caster?.TotalArmor ?? 0;

        // Execute card effect
        int actionCountBeforeEffect = savedGame.ActionLog.Count;
        var effect = CardEffectFactory.GetEffect(card.CardId);
        effect.Execute(card, partyMemberTargets, enemyTargets, savedGame);
        var playActions = savedGame.ActionLog.Skip(actionCountBeforeEffect).ToList();

        // Move card to discard pile ONLY IF it was NOT added to the board
        bool isOnBoard = savedGame.Board.Any(b => b.Card.InstanceId == card.InstanceId);
        if (!isOnBoard)
        {
            savedGame.Players[playerIndex].DiscardPile.Add(card);
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.Discard, savedGame, cards: new List<CardDto> { card }));
        }

        CombatTurnHelper.ProcessBoardEffects(savedGame, EntryPointType.AfterCardPlayed, card.PartyMemberId, playActions);

        ApplyWarriorMomentumArmorShare(savedGame, caster, actionLogCountBefore, casterArmorBefore);
        ApplyAssassinDaggers(savedGame, caster, card, enemyTargets);

        var newlyDeadEnemies = savedGame.Enemies
            .Where(e => e.HitPoints <= 0 && !deadEnemiesBeforeEffects.Contains(e.InstanceId))
            .ToList();

        if (newlyDeadEnemies.Count > 0)
        {
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.EnemyDied,
                savedGame,
                enemies: newlyDeadEnemies));
        }

        static (InitiativeType Type, Guid? EnemyInstanceId, int? PlayerInstanceId, string Name, int Speed)
            ToComparableInitiative(InitiativeDto initiative) =>
                (initiative.Type, initiative.EnemyInstanceId, initiative.PlayerInstanceId, initiative.Name, initiative.Speed);

        var initiativeBeforeCleanup = savedGame.Initiatives
            .Select(ToComparableInitiative)
            .ToList();

        savedGame.Initiatives = CombatTurnHelper.RemoveDefeatedFromInitiative(savedGame);
        CombatTurnHelper.SynchronizeInitiativeSpeeds(savedGame);

        var initiativeChanged = !savedGame.Initiatives
            .Select(ToComparableInitiative)
            .SequenceEqual(initiativeBeforeCleanup);

        if (initiativeChanged)
            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(GameActionType.UpdateInitiative, savedGame));
        CombatTurnHelper.AppendRoomWonIfApplicable(savedGame);

        var actionsToReturn = savedGame.ActionLog.ToList();

        if (actionsToReturn.Any(a => a.Type == GameActionType.RoomWon))
        {
            var room = await roomRepository.GetByDungeonIdAsync(game.Dungeon.Id, cancellationToken);
            if (room is not null)
            {
                room.Update(RoomType.EncounterReward, room.Reward, room.Seed, room.EnemyIds, room.BackgroundId);
                await roomRepository.UpdateAsync(room, cancellationToken);
            }

            var cardUseCounts = savedGame.Players
                .SelectMany(p => p.DrawPile.Concat(p.Hand).Concat(p.DiscardPile))
                .Where(c => c.UseCount > 0)
                .GroupBy(c => c.InstanceId)
                .ToDictionary(g => g.Key, g => g.Sum(c => c.UseCount));

            if (cardUseCounts.Count > 0)
            {
                var deckByInstanceId = game.Players
                    .SelectMany(p => p.Deck)
                    .ToDictionary(d => d.InstanceId);

                foreach (var (instanceId, encounterUseCount) in cardUseCounts)
                {
                    if (deckByInstanceId.TryGetValue(instanceId, out var deckCard))
                        deckCard.IncrementUseCount(encounterUseCount);
                }
            }

            savedGame.ActionLog.Clear();
        }

        game.SetSave(savedGame.ToJson());
        await gameRepository.UpdateAsync(game, cancellationToken);

        return actionsToReturn;
    }

    private void ApplyAssassinDaggers(SavedGameDto savedGame, PartyMemberDto? caster, CardDto card, List<EnemyDto> enemyTargets)
    {
        if (caster?.ClassName == "Assassin" && card.Daggers.HasValue && card.Daggers.Value != 0)
        {
            int previousDaggers = caster.Daggers ?? 0;
            caster.Daggers = Math.Max(0, previousDaggers + card.Daggers.Value);

            savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                GameActionType.Daggers,
                savedGame,
                partyMembers: new List<PartyMemberDto> { caster },
                enemies: new List<EnemyDto>()));

            if (previousDaggers > 0 && caster.Daggers == 0 && enemyTargets.Count > 0)
            {
                foreach (var enemy in enemyTargets)
                {
                    enemy.ApplyPoison(2, caster.Identifier);
                }
                savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                    GameActionType.Poison,
                    savedGame,
                    enemies: enemyTargets));
            }
        }
    }

    private void ApplyWarriorMomentumArmorShare(SavedGameDto savedGame, PartyMemberDto? caster, int actionLogCountBefore, int casterArmorBefore)
    {
        if (caster?.ClassName == "Warrior" && caster.Momentum >= 4)
        {
            var actionsAdded = savedGame.ActionLog.Skip(actionLogCountBefore).ToList();
            bool addedArmor = actionsAdded.Any(a => a.Type == GameActionType.Armor);

            if (addedArmor)
            {
                int armorGained = caster.TotalArmor - casterArmorBefore;
                if (armorGained > 0)
                {
                    caster.ApplyMomentum(-4);
                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                        GameActionType.MomentumRemove,
                        savedGame,
                        partyMembers: new List<PartyMemberDto> { caster }));

                    var aliveMembers = savedGame.PartyMembers.Where(pm => pm.Health > 0).ToList();
                    foreach (var pm in aliveMembers)
                    {
                        pm.Armor.Add(new EffectEntry(armorGained, caster.Identifier));
                    }

                    savedGame.ActionLog.Add(CombatTurnHelper.BuildAction(
                        GameActionType.Armor,
                        savedGame,
                        partyMembers: aliveMembers));
                }
            }
        }
    }
}
