using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Decks;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;
using MysticRealms.Domain.Games;
using MysticRealms.Domain.Players;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.NextRoom;

public class NextRoomCommandHandler(
    IGameRepository gameRepository,
    IDungeonRepository dungeonRepository,
    IPartyMemberRepository partyMemberRepository,
    IEnemyRepository enemyRepository,
    IRoomRepository roomRepository,
    DungeonGenerator dungeonGenerator) : IRequestHandler<NextRoomCommand, List<GameAction>>
{
    private static readonly RoomType[] CombatRoomTypes =
        [RoomType.Encounter, RoomType.Elite, RoomType.Boss];

    public async Task<List<GameAction>> Handle(NextRoomCommand request, CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);

        var dungeon = game!.Dungeon;
        
        var existingRoom = await roomRepository.GetByDungeonIdAsync(dungeon.Id, cancellationToken);

        var option = string.IsNullOrEmpty(request.Option) ? RoomOptions.Initial : request.Option;

        dungeon.NextRoom(option);
        await dungeonRepository.UpdateAsync(dungeon, cancellationToken);

        var room = dungeonGenerator.GenerateRoom(dungeon.Path, dungeon.Level, dungeon.Id);
        
        if (existingRoom is null)
            await roomRepository.AddAsync(room, cancellationToken);
        else
        {
            existingRoom.Update(room.Type, room.Reward, room.Seed, room.EnemyIds, room.BackgroundId);
            await roomRepository.UpdateAsync(existingRoom, cancellationToken);
            room = existingRoom;
        }

        var enemySaves = await GetEnemies(room, cancellationToken);
        var partyMemberSaves = await GetPartyMembers(request.GameId, cancellationToken);
        var playerSaves = GetPlayers(game.Players.ToList(), partyMemberSaves);

        var savedGame = new SavedGameDto();
        savedGame.CurrentRoom = new RoomDto(room.Type, room.Reward, room.Seed, room.BackgroundId);
        savedGame.PartyMembers = partyMemberSaves;
        savedGame.Enemies = enemySaves;
        savedGame.Players = playerSaves;

        foreach (var member in savedGame.PartyMembers)
        {
            if (member.ClassName == "Assassin")
            {
                member.Daggers = 3;
            }
        }
        
        
        var gameActions = new List<GameAction>();
        gameActions.Add(CombatTurnHelper.BuildAction(
            GameActionType.SetInitial,
            savedGame
            ));
        savedGame.ActionLog = gameActions;
        game.SetSave(savedGame.ToJson());
        await gameRepository.UpdateAsync(game, cancellationToken);

        return savedGame.ActionLog;
    }

    private static List<PlayerDto> GetPlayers(List<Player> players, List<PartyMemberDto> partyMembers)
    {
        var classIdByPartyMemberId = partyMembers.ToDictionary(pm => pm.PartyMemberId, pm => pm.ClassId);
        var result = new List<PlayerDto>();
        foreach (var player in players)
        {
            var deck = player.Deck.Select(d =>
            {
                if (!classIdByPartyMemberId.TryGetValue(d.PartyMemberId, out var classId))
                    throw new InvalidOperationException($"Missing class for party member {d.PartyMemberId} on deck card {d.Id}.");

                return new CardDto(
                    d.Id,
                    player.Id,
                    d.CardId,
                    d.InstanceId,
                    d.PartyMemberId,
                    classId,
                    d.Color,
                    d.Card.Name,
                    d.Card.Description,
                    d.Card.Rarity,
                    d.IsAscended,
                    0,
                    d.ManaCost,
                    d.Attack,
                    d.MagicAttack,
                    d.Armor,
                    d.MagicArmor,
                    d.Mana,
                    d.Strength,
                    d.Defense,
                    d.Draw,
                    d.EffectMultiplier,
                    d.Heal,
                    d.Poison,
                    d.LifeCost,
                    d.PlayerTargets,
                    d.EnemyTargets,
                    d.Card.Permanent ??  false,
                    d.Card.Momentum,
                    d.Card.Daggers,
                    d.Card.Spirit,
                    d.Card.Fire,
                    d.Card.Water,
                    d.Card.Earth,
                    d.Card.Air
                    );
            }).ToList();
            var mana = (int)Math.Round(player.PartyMembers.Average(p => p.Mana));
            var speed = (int)Math.Round(player.PartyMembers.Average(p => p.Speed));
            var playerDto = new PlayerDto(player.Id, player.UserId, deck, ShuffleDeck(deck), [], [], mana, speed, player.User.GamerTag);
            result.Add(playerDto);
        }
        return result;
    }

    private async Task<List<PartyMemberDto>> GetPartyMembers(int gameId, CancellationToken cancellationToken)
    {
        var partyMembers = await partyMemberRepository.GetByGameIdAsync(gameId, cancellationToken);
        return partyMembers
            .Select(pm => new PartyMemberDto(pm.Id, pm.PlayerId, pm.InstanceId, pm.Color, pm.ClassId, pm.Gender, pm.Health,
                pm.MaxHealth, pm.Mana, pm.Speed, pm.Class?.Name ?? string.Empty, spirit: pm.Spirit, fire: pm.Fire, water: pm.Water, earth: pm.Earth, air: pm.Air))
            .ToList();
    }

    private async Task<List<EnemyDto>> GetEnemies(Room room, CancellationToken cancellationToken)
    {
        var enemies = new List<EnemyDto>();
        if (CombatRoomTypes.Contains(room.Type) && room.EnemyIds.Length > 0)
        {
            var enemyTemplates = await enemyRepository.GetByIdsAsync(room.EnemyIds, cancellationToken);
            var enemyLookup = enemyTemplates.ToDictionary(e => e.Id);

            var instanceRng = new Random(DungeonGenerator.GetStableHash(room.Seed));

            enemies = room.EnemyIds.Select(id =>
            {
                var e = enemyLookup[id];
                int minHp = Math.Min(e.MinHitPoints, e.MaxHitPoints);
                int maxHp = Math.Max(e.MinHitPoints, e.MaxHitPoints);
                int hp = instanceRng.Next(minHp, maxHp + 1);
                int minSpd = Math.Min(e.MinSpeed, e.MaxSpeed);
                int maxSpd = Math.Max(e.MinSpeed, e.MaxSpeed);
                int speed = instanceRng.Next(minSpd, maxSpd + 1);
                return new EnemyDto(Guid.NewGuid(), e.Id, e.Name, hp, hp, speed);
            }).ToList();
        }
        return enemies;
    }

    private static List<CardDto> ShuffleDeck(List<CardDto> deck)
    {
        var shuffleBytes = new byte[deck.Count * 4];
        System.Security.Cryptography.RandomNumberGenerator.Fill(shuffleBytes);
        
        var drawPile = deck
            .Select((card, i) => new
            {
                Card = card,
                Key = BitConverter.ToInt32(shuffleBytes, i * 4)
            })
            .OrderBy(x => x.Key)
            .Select(x => x.Card)
            .ToList();

        return drawPile;
    }

    
}
