using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Decks;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands;

/// <summary>
/// Shared turn-processing helpers used by both <see cref="NextRoom.NextRoomCommandHandler"/>
/// and <see cref="NextTurn.NextTurnCommandHandler"/>.
/// </summary>
internal static class CombatTurnHelper
{
    internal static GameAction BuildAction(
        GameActionType type,
        SavedGameDto savedGame,
        int? turn = null,
        IList<CardDto>? cards = null,
        IList<PartyMemberDto>? partyMembers = null,
        IList<EnemyDto>? enemies = null)
    {
        var players = savedGame.Players.Select(s => new PlayerGameAction(
            s.Id,
            s.UserId,
            s.Hand.Select(ToRecord).ToList(),
            s.Mana
        )).ToList();

        var initiative = savedGame.Initiatives.Select(ToRecord).ToList();
        var partyMemberRecords = (partyMembers ?? savedGame.PartyMembers).Select(ToRecord).ToList();
        var enemyRecords = (enemies ?? savedGame.Enemies).Select(ToRecord).ToList();
        var room = savedGame.CurrentRoom is not null ? ToRecord(savedGame.CurrentRoom) : null;
        var cardRecords = cards?.Select(ToRecord).ToList();
        var boardRecords = savedGame.Board.Select(ToRecord).ToList();

        return new GameAction(type, turn, players, initiative, cardRecords, partyMemberRecords, enemyRecords, room, boardRecords);
    }

    private static InitiativeRecord ToRecord(InitiativeDto i) =>
        new(i.Type, i.EnemyInstanceId, i.PlayerInstanceId, i.Name, i.Speed);

    private static PartyMemberRecord ToRecord(PartyMemberDto pm) =>
        new(pm.PartyMemberId, pm.PlayerId, pm.Identifier, pm.Color, pm.ClassId, pm.Gender,
            pm.Health, pm.MaxHealth, pm.Mana, pm.Speed, pm.ClassName, pm.TotalArmor,
            pm.TotalMagicArmor, pm.Strength, pm.Defense, pm.Momentum, pm.Daggers, pm.Spirit, pm.Fire, pm.Water, pm.Earth, pm.Air);

    private static EnemyRecord ToRecord(EnemyDto e) =>
        new(e.InstanceId, e.Id, e.Name, e.HitPoints, e.MaxHitPoints, e.Speed, e.TotalArmor, e.TotalMagicArmor, e.TotalPoison);

    internal static CardRecord ToRecord(CardDto c) =>
        new(c.Id, c.PlayerId, c.CardId, c.InstanceId, c.PartyMemberId, c.Color, c.Name,
            c.Description, c.Rarity, c.IsAscended, c.UseCount, c.ManaCost, c.Attack, c.MagicAttack, c.Armor, c.MagicArmor, c.Mana,
            c.Strength, c.Defense, c.Draw, c.EffectMultiplier, c.Heal, c.Poison, c.LifeCost,
            c.PlayerTargets, c.EnemyTargets, c.Permanent, c.ClassId, c.Momentum, c.Daggers, c.Spirit, c.Fire, c.Water, c.Earth, c.Air);

    private static RoomRecord ToRecord(RoomDto r) =>
        new(r.Type, r.Reward, r.Key, r.BackgroundId);

    private static BoardCardSnapshot ToRecord(BoardCardRecord b) =>
        new(ToRecord(b.Card), b.PartyMemberId, b.EntryPoint);

    internal static int GenerateMana(List<PartyMemberDto>? partyMembers = null)
    {
        if (partyMembers == null)
            return 0;
        return (int)Math.Round(partyMembers.Average(s => s.Mana));
    }

    internal static int GenerateSpeed(List<PartyMemberDto>? partyMembers = null)
    {
        if (partyMembers == null)
            return 0;
        return (int)Math.Round(partyMembers.Average(s => s.Speed));
    }

    internal static List<InitiativeDto> GetInitiative(SavedGameDto savedGame)
    {
        var firstActor = savedGame.Initiatives.FirstOrDefault();
        savedGame.Initiatives = RemoveDefeatedFromInitiative(savedGame);

        if (savedGame.Initiatives.Count == 0)
            return GenerateInitiativeStack(savedGame);

        // Only skip if the current actor is still at the front of the list after removing defeated.
        // If they were removed because they were defeated, the list has already "advanced".
        if (firstActor != null && savedGame.Initiatives.Count > 0 && savedGame.Initiatives[0] == firstActor)
        {
            savedGame.Initiatives = savedGame.Initiatives.Skip(1).ToList();
        }

        savedGame.Initiatives = RemoveDefeatedFromInitiative(savedGame);

        SynchronizeInitiativeSpeeds(savedGame);

        return savedGame.Initiatives;
    }

    internal static void SynchronizeInitiativeSpeeds(SavedGameDto savedGame)
    {
        // 1. Update Player speeds based on alive party members
        foreach (var player in savedGame.Players)
        {
            var aliveMembers = savedGame.PartyMembers
                .Where(w => w.Health > 0 && w.PlayerId == player.Id)
                .ToList();

            player.Speed = aliveMembers.Count > 0
                ? (int)Math.Round(aliveMembers.Average(s => s.Speed))
                : 0;
        }

        // 2. Update speeds in Initiatives
        foreach (var initiative in savedGame.Initiatives)
        {
            if (initiative.Type == InitiativeType.Player)
            {
                var player = savedGame.Players.FirstOrDefault(p => p.Id == initiative.PlayerInstanceId);
                if (player != null)
                {
                    initiative.Speed = player.Speed;
                }
            }
            else if (initiative.Type == InitiativeType.Enemy)
            {
                var enemy = savedGame.Enemies.FirstOrDefault(e => e.InstanceId == initiative.EnemyInstanceId);
                if (enemy != null)
                {
                    initiative.Speed = enemy.Speed;
                }
            }
        }

        // 3. Re-sort remaining initiatives (index 1+) by Speed DESC
        if (savedGame.Initiatives.Count > 1)
        {
            var currentActor = savedGame.Initiatives[0];
            var remaining = savedGame.Initiatives.Skip(1).OrderByDescending(i => i.Speed).ToList();
            savedGame.Initiatives = new List<InitiativeDto> { currentActor };
            savedGame.Initiatives.AddRange(remaining);
        }
    }

    internal static List<InitiativeDto> GenerateInitiativeStack(SavedGameDto savedGame)
    {
        var entries = new List<InitiativeDto>();

        for (int i = 0; i < savedGame.Players.Count; i++)
        {
            var player = savedGame.Players[i];

            var aliveMembers = savedGame.PartyMembers
                .Where(w => w.Health > 0 && w.PlayerId == player.Id)
                .ToList();

            var speed = aliveMembers.Count > 0
                ? (int)Math.Round(aliveMembers.Average(s => s.Speed))
                : 0;

            player.Speed = speed;
            if (aliveMembers.Count == 0)
                continue;

            entries.Add(new InitiativeDto(InitiativeType.Player, null, player.Id, player.GamerTag, player.Speed));
        }
    
        foreach (var enemy in savedGame.Enemies.Where(e => e.HitPoints > 0))
            entries.Add(new InitiativeDto(InitiativeType.Enemy, enemy.InstanceId, null, enemy.Name, enemy.Speed));
    
        return entries.OrderByDescending(e => e.Speed).ToList();
    }

    internal static List<InitiativeDto> RemoveDefeatedFromInitiative(SavedGameDto savedGame)
    {
        var aliveEnemyIds = savedGame.Enemies
            .Where(e => e.HitPoints > 0)
            .Select(e => e.InstanceId)
            .ToHashSet();

        var activePlayerIds = savedGame.PartyMembers
            .Where(pm => pm.Health > 0)
            .Select(pm => pm.PlayerId)
            .ToHashSet();

        return savedGame.Initiatives
            .Where(i => i.Type switch
            {
                InitiativeType.Enemy => i.EnemyInstanceId is Guid enemyInstanceId &&
                                        aliveEnemyIds.Contains(enemyInstanceId),
                InitiativeType.Player => i.PlayerInstanceId is int playerId &&
                                         activePlayerIds.Contains(playerId),
                _ => false
            })
            .ToList();
    }
    //
    // /// <summary>
    // /// Discards the current hand, draws a full hand, and appends the corresponding
    // /// <see cref="GameActionType.DiscardHand"/> / <see cref="GameActionType.Draw"/> /
    // /// <see cref="GameActionType.ShuffleDeck"/> actions.
    // /// Returns the updated <see cref="EncounterState"/>.
    // /// </summary>
    // internal static EncounterState ProcessPlayerTurn(
    //     EncounterState encounter,
    //     SavedGameDto savedGame,
    //     List<GameAction> actions)
    // {
    //     var hand = encounter.Hand.ToList();
    //     var drawPile = encounter.DrawPile.ToList();
    //     var discard = encounter.DiscardPile.ToList();
    //
    //     // Discard current hand — include card data so the client can animate discards
    //     var discardedCards = hand.Select(id => LookupActionCard(savedGame, id)).ToList();
    //     actions.Add(BuildAction(GameActionType.DiscardHand, savedGame, cards: discardedCards));
    //     discard.AddRange(hand);
    //     hand.Clear();
    //
    //     // Draw up to DefaultHandSize cards from the draw pile
    //     hand = DrawCards(DefaultHandSize, drawPile, discard, savedGame, actions);
    //
    //     return new EncounterState(DrawPile: drawPile, Hand: hand, DiscardPile: discard);
    // }
    //
    // /// <summary>
    // /// Draws <paramref name="count"/> cards from <paramref name="drawPile"/>, shuffling
    // /// <paramref name="discard"/> into the draw pile when it runs out, and appending
    // /// the matching actions to <paramref name="actions"/>.
    // /// </summary>
    // internal static List<int> DrawCards(
    //     int count,
    //     List<int> drawPile,
    //     List<int> discard,
    //     SavedGameDto savedGame,
    //     List<GameAction> actions)
    // {
    //     var hand = new List<int>();
    //     var remaining = count;
    //
    //     if (drawPile.Count >= remaining)
    //     {
    //         var drawn = drawPile.Take(remaining).ToList();
    //         drawPile.RemoveRange(0, remaining);
    //         actions.Add(BuildAction(GameActionType.Draw, savedGame, cards: ToCardList(savedGame, drawn)));
    //         hand.AddRange(drawn);
    //     }
    //     else
    //     {
    //         // Draw whatever is left in the draw pile
    //         if (drawPile.Count > 0)
    //         {
    //             var firstBatch = drawPile.ToList();
    //             remaining -= firstBatch.Count;
    //             drawPile.Clear();
    //             actions.Add(BuildAction(GameActionType.Draw, savedGame, cards: ToCardList(savedGame, firstBatch)));
    //             hand.AddRange(firstBatch);
    //         }
    //
    //         // Shuffle discard pile into draw pile using a CSPRNG
    //         var shuffleBytes = new byte[discard.Count * 4];
    //         System.Security.Cryptography.RandomNumberGenerator.Fill(shuffleBytes);
    //         var shuffled = discard
    //             .Select((id, i) => (id, key: BitConverter.ToInt32(shuffleBytes, i * 4)))
    //             .OrderBy(x => x.key)
    //             .Select(x => x.id)
    //             .ToList();
    //         discard.Clear();
    //         drawPile.AddRange(shuffled);
    //         actions.Add(BuildAction(GameActionType.ShuffleDeck, savedGame));
    //
    //         // Draw the remaining cards
    //         var secondBatch = drawPile.Take(remaining).ToList();
    //         drawPile.RemoveRange(0, secondBatch.Count);
    //         if (secondBatch.Count > 0)
    //             actions.Add(BuildAction(GameActionType.Draw, savedGame, cards: ToCardList(savedGame, secondBatch)));
    //         hand.AddRange(secondBatch);
    //     }
    //
    //     return hand;
    // }

    /// <summary>
    /// Runs the first-turn initialization for a freshly entered room:
    /// emits a <see cref="GameActionType.SetInitial"/> snapshot, generates the initiative
    /// stash, auto-advances past leading enemy entries, processes the first player turn
    /// (discard empty hand + draw), and returns the resulting stash, encounter state,
    /// turn number, and actions.
    /// </summary>
    // internal static (int Turn, List<InitiativeDto> Stash, List<GameAction> Actions)
    //     InitializeFirstTurn(SavedGameDto savedGame)
    // {
    //     var actions = new List<GameAction>();
    //     var stash = new List<InitiativeEntry>();
    //     var turn = 0;
    //
    //     // Stash starts empty — begin turn 1
    //     turn++;
    //     stash = GenerateInitiativeStash(savedGame);
    //
    //     // Emit SetInitial as the very first action so the client has a known baseline
    //     actions.Add(BuildSetInitialAction(savedGame, turn, stash, encounter.Hand));
    //
    //     actions.Add(BuildAction(GameActionType.UpdateTurn, savedGame, turn: turn));
    //
    //     // Auto-advance through any leading enemy entries
    //     while (stash.Count > 0 && stash[0].Type == InitiativeType.Enemy)
    //     {
    //         actions.Add(BuildAction(GameActionType.UpdateInitiative, savedGame,
    //             initiative: stash.ToList()));
    //         stash.RemoveAt(0);
    //
    //         if (stash.Count == 0)
    //         {
    //             turn++;
    //             stash = GenerateInitiativeStash(savedGame);
    //             actions.Add(BuildAction(GameActionType.UpdateTurn, savedGame, turn: turn));
    //         }
    //     }
    //
    //     // Process the first player entry
    //     if (stash.Count > 0 && stash[0].Type == InitiativeType.Player)
    //     {
    //         actions.Add(BuildAction(GameActionType.UpdateInitiative, savedGame,
    //             initiative: stash.ToList()));
    //         encounter = ProcessPlayerTurn(encounter, savedGame, actions);
    //     }
    //
    //     return (turn, stash, encounter, actions);
    // }

    /// <summary>
    /// Discards the current hand of the player at <paramref name="playerIndex"/>, draws
    /// <paramref name="cardCount"/> cards from their draw pile (shuffling the discard pile
    /// back in when the draw pile runs out), and appends the corresponding
    /// <see cref="GameActionType.DiscardHand"/>, <see cref="GameActionType.Draw"/>, and
    /// <see cref="GameActionType.ShuffleDeck"/> actions to <paramref name="savedGame"/>.
    /// </summary>
    internal static void DiscardAndDraw(SavedGameDto savedGame, int playerIndex, int cardCount)
    {
        var player = savedGame.Players[playerIndex];

        // Discard current hand
        var discardedCards = player.Hand.ToList();
        var newDiscardPile = player.DiscardPile.Concat(discardedCards).ToList();
        player.Hand = new List<CardDto>();
        player.DiscardPile = newDiscardPile;
        savedGame.ActionLog.Add(BuildAction(GameActionType.DiscardHand, savedGame, cards: discardedCards));

        DrawCardsForPlayer(savedGame, playerIndex, cardCount);
    }

    /// <summary>
    /// Draws <paramref name="cardCount"/> cards from the draw pile of the player at
    /// <paramref name="playerIndex"/> into their hand, shuffling the discard pile back
    /// when the draw pile runs out, and adds the corresponding
    /// <see cref="GameActionType.Draw"/> and <see cref="GameActionType.ShuffleDeck"/>
    /// entries to <paramref name="savedGame"/>.<see cref="SavedGameDto.ActionLog"/>.
    /// </summary>
    internal static void DrawCardsForPlayer(SavedGameDto savedGame, int playerIndex, int cardCount)
    {
        var drawPile = savedGame.Players[playerIndex].DrawPile.ToList();
        var discardPile = savedGame.Players[playerIndex].DiscardPile.ToList();
        var hand = savedGame.Players[playerIndex].Hand.ToList();
        var remaining = cardCount;

        if (drawPile.Count >= remaining)
        {
            var drawn = drawPile.Take(remaining).ToList();
            drawPile.RemoveRange(0, remaining);
            hand.AddRange(drawn);
            savedGame.Players[playerIndex].DrawPile = drawPile;
            savedGame.Players[playerIndex].Hand = hand;
            savedGame.ActionLog.Add(BuildAction(GameActionType.Draw, savedGame, cards: drawn));
        }
        else
        {
            if (drawPile.Count > 0)
            {
                var firstBatch = drawPile.ToList();
                remaining -= firstBatch.Count;
                hand.AddRange(firstBatch);
                drawPile.Clear();
                savedGame.Players[playerIndex].DrawPile = drawPile;
                savedGame.Players[playerIndex].Hand = hand;
                savedGame.ActionLog.Add(BuildAction(GameActionType.Draw, savedGame, cards: firstBatch));
            }

            // Shuffle discard pile into draw pile
            var shuffleBytes = new byte[discardPile.Count * 4];
            System.Security.Cryptography.RandomNumberGenerator.Fill(shuffleBytes);
            var shuffled = discardPile
                .Select((card, i) => (card, key: BitConverter.ToInt32(shuffleBytes, i * 4)))
                .OrderBy(x => x.key)
                .Select(x => x.card)
                .ToList();
            discardPile.Clear();
            drawPile.AddRange(shuffled);
            savedGame.Players[playerIndex].DrawPile = drawPile;
            savedGame.Players[playerIndex].DiscardPile = discardPile;
            savedGame.ActionLog.Add(BuildAction(GameActionType.ShuffleDeck, savedGame));

            // Draw remaining cards
            var secondBatch = drawPile.Take(remaining).ToList();
            drawPile.RemoveRange(0, secondBatch.Count);
            hand.AddRange(secondBatch);
            savedGame.Players[playerIndex].DrawPile = drawPile;
            savedGame.Players[playerIndex].Hand = hand;
            if (secondBatch.Count > 0)
                savedGame.ActionLog.Add(BuildAction(GameActionType.Draw, savedGame, cards: secondBatch));
        }
    }

    /// <summary>
    /// Removes all <see cref="EffectEntry"/> armor entries sourced by any party member
    /// belonging to <paramref name="playerId"/> from every party member in the game.
    /// If any entries were removed, appends a <see cref="GameActionType.Armor"/> snapshot
    /// to the action log.
    /// </summary>
    internal static void ClearArmorForPlayer(SavedGameDto savedGame, int playerId)
    {
        var sourceIds = savedGame.PartyMembers
            .Where(pm => pm.PlayerId == playerId)
            .Select(pm => pm.Identifier)
            .ToHashSet();

        var anyRemoved = false;
        var updatedPartyMembers = new List<PartyMemberDto>();
        foreach (var pm in savedGame.PartyMembers)
        {
            var before = pm.Armor.Count;
            var beforeMagic = pm.MagicArmor.Count;
            pm.Armor.RemoveAll(e => sourceIds.Contains(e.Source));
            pm.MagicArmor.RemoveAll(e => sourceIds.Contains(e.Source));
            if (pm.Armor.Count != before || pm.MagicArmor.Count != beforeMagic)
            {
                anyRemoved = true;
                updatedPartyMembers.Add(pm);
            }
        }

        if (anyRemoved)
            savedGame.ActionLog.Add(BuildAction(GameActionType.Armor, savedGame, partyMembers: updatedPartyMembers));
    }

    /// <summary>
    /// Removes all <see cref="EffectEntry"/> armor entries sourced by <paramref name="enemyInstanceId"/>
    /// from every enemy in the game.
    /// If any entries were removed, appends a <see cref="GameActionType.EnemyArmor"/> snapshot
    /// to the action log.
    /// </summary>
    internal static void ClearArmorForEnemy(SavedGameDto savedGame, Guid enemyInstanceId)
    {
        var anyRemoved = false;
        var updatedEnemies = new List<EnemyDto>();
        foreach (var enemy in savedGame.Enemies)
        {
            var before = enemy.Armor.Count;
            var beforeMagic = enemy.MagicArmor.Count;
            enemy.Armor.RemoveAll(e => e.Source == enemyInstanceId);
            enemy.MagicArmor.RemoveAll(e => e.Source == enemyInstanceId);
            if (enemy.Armor.Count != before || enemy.MagicArmor.Count != beforeMagic)
            {
                anyRemoved = true;
                updatedEnemies.Add(enemy);
            }
        }

        if (anyRemoved)
            savedGame.ActionLog.Add(BuildAction(GameActionType.EnemyArmor, savedGame, enemies: updatedEnemies));
    }

    internal static void MoveBoardCardsToDiscardPile(SavedGameDto savedGame)
    {
        foreach (var boardCard in savedGame.Board)
        {
            var player = savedGame.Players.FirstOrDefault(p => p.Id == boardCard.Card.PlayerId);
            if (player != null)
            {
                player.DiscardPile.Add(boardCard.Card);
            }
        }
        savedGame.Board.Clear();
    }

    internal static void AppendRoomWonIfApplicable(SavedGameDto savedGame)
    {
        if (savedGame.Enemies.Count == 0)
            return;

        if (!savedGame.Enemies.All(e => e.HitPoints <= 0))
            return;

        if (savedGame.ActionLog.Any(a => a.Type == GameActionType.RoomWon))
            return;

        MoveBoardCardsToDiscardPile(savedGame);
        savedGame.ActionLog.Add(BuildAction(GameActionType.RoomWon, savedGame));
    }

    internal static bool ConsumePoisonForEnemy(SavedGameDto savedGame, Guid enemyInstanceId)
    {
        var enemy = savedGame.Enemies.FirstOrDefault(e => e.InstanceId == enemyInstanceId);
        if (enemy == null || enemy.HitPoints <= 0 || enemy.TotalPoison <= 0) return false;

        int poisonDamage = enemy.TotalPoison;
        enemy.HitPoints = Math.Max(0, enemy.HitPoints - poisonDamage);

        var firstPoison = enemy.Poison.FirstOrDefault();
        if (firstPoison != null)
        {
            firstPoison.Amount--;
            if (firstPoison.Amount <= 0) enemy.Poison.Remove(firstPoison);
        }

        savedGame.ActionLog.Add(BuildAction(
            GameActionType.PoisonConsumed,
            savedGame,
            enemies: new List<EnemyDto> { enemy }
        ));

        if (enemy.HitPoints <= 0)
        {
            savedGame.ActionLog.Add(BuildAction(
                GameActionType.EnemyDied,
                savedGame,
                enemies: new List<EnemyDto> { enemy }));

            return true;
        }

        return false;
    }

    internal static void ProcessBoardEffects(SavedGameDto savedGame, EntryPointType entryPoint, int casterPartyMemberId, List<GameAction> playActions)
    {
        var boardCards = savedGame.Board
            .Where(b => b.PartyMemberId == casterPartyMemberId && b.EntryPoint == entryPoint)
            .ToList();

        foreach (var boardCard in boardCards)
        {
            var effect = BoardEffectFactory.GetEffect(boardCard.Card.CardId);
            effect.React(boardCard, savedGame, playActions);
        }
    }

    internal static bool ConsumePoisonForPlayer(SavedGameDto savedGame, int playerId)
    {
        var playerPartyMembers = savedGame.PartyMembers
            .Where(pm => pm.PlayerId == playerId && pm.Health > 0)
            .ToList();

        var poisonedMembers = new List<PartyMemberDto>();
        var newlyDeadPartyMembers = new List<PartyMemberDto>();

        foreach (var pm in playerPartyMembers)
        {
            if (pm.TotalPoison > 0)
            {
                int poisonDamage = pm.TotalPoison;
                pm.Health = Math.Max(0, pm.Health - poisonDamage);

                var firstPoison = pm.Poison.FirstOrDefault();
                if (firstPoison != null)
                {
                    firstPoison.Amount--;
                    if (firstPoison.Amount <= 0) pm.Poison.Remove(firstPoison);
                }

                poisonedMembers.Add(pm);

                if (pm.Health <= 0)
                {
                    newlyDeadPartyMembers.Add(pm);
                }
            }
        }

        if (poisonedMembers.Count > 0)
        {
            savedGame.ActionLog.Add(BuildAction(
                GameActionType.PoisonConsumed,
                savedGame,
                partyMembers: poisonedMembers
            ));
        }

        foreach (var deadMember in newlyDeadPartyMembers)
        {
            savedGame.ActionLog.Add(BuildAction(
                GameActionType.Died,
                savedGame,
                partyMembers: new List<PartyMemberDto> { deadMember }));
        }

        var aliveMembers = savedGame.PartyMembers
            .Where(w => w.Health > 0 && w.PlayerId == playerId)
            .ToList();

        return aliveMembers.Count == 0;
    }
}
