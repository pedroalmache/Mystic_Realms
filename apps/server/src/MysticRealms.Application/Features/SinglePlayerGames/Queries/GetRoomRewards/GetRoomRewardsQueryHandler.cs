using MediatR;
using System.Security.Cryptography;
using System.Text;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Blessings;
using MysticRealms.Domain.Cards;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomRewards;

public class GetRoomRewardsQueryHandler(
    IGameRepository gameRepository,
    IRoomRepository roomRepository,
    ICardRepository cardRepository,
    IBlessingRepository blessingRepository,
    IPartyMemberRepository partyMemberRepository)
    : IRequestHandler<GetRoomRewardsQuery, GetRoomRewardsResult?>
{
    private const int GoldPerMember = 15;
    private const int CardsPerPlayer = 3;
    private const int BlessingCount = 3;
    private const int MaxBlessingRarity = 3;

    // Weights for blessing rarities (index 0 = rarity 1, index 1 = rarity 2, index 2 = rarity 3)
    private static readonly int[] BlessingRarityWeights = [6, 3, 1];

    public async Task<GetRoomRewardsResult?> Handle(
        GetRoomRewardsQuery request,
        CancellationToken cancellationToken)
    {
        var game = await gameRepository.GetByIdAsync(request.GameId, cancellationToken);
        var save = SavedGameDto.FromJson(game!.Save)!;

        var room = await roomRepository.GetByDungeonIdAsync(game.Dungeon.Id, cancellationToken);
        if (room?.Type != RoomType.EncounterReward)
            return null;

        var dungeon = game.Dungeon;
        var baseSeed = DungeonGenerator.GetStableHash(dungeon.Path);

        var partyMembers = await partyMemberRepository.GetByGameIdAsync(request.GameId, cancellationToken);
        var classCardsByClassId = new Dictionary<int, List<Card>>();
        foreach (var classId in partyMembers.Select(pm => pm.ClassId).Distinct())
        {
            var classCards = await cardRepository.GetByClassIdAsync(classId, cancellationToken);
            classCardsByClassId[classId] = classCards
                .Where(c => c.Rarity > 0)
                .ToList();
        }

        var playerGroups = partyMembers
            .GroupBy(pm => pm.Player.Id)
            .OrderBy(group => group.Key)
            .ToList();
        var playerRewards = new List<PlayerReward>();

        foreach (var playerGroup in playerGroups)
        {
            var playerId = playerGroup.Key;
            var members = playerGroup.OrderBy(pm => pm.Id).ToList();
            var cardCandidates = new List<(Domain.PartyMembers.PartyMember Member, Card Card)>();

            foreach (var member in members)
            {
                cardCandidates.AddRange(classCardsByClassId[member.ClassId]
                    .Select(c => (member, c)));
            }

            var playerCardRng = new Random(DungeonGenerator.GetStableHash($"{dungeon.Path}_cards_player_{playerId}"));
            var pickedCards = PickCards(cardCandidates, CardsPerPlayer, playerCardRng);

            var rewardCards = pickedCards
                .Select((tuple, index) => ToRewardCardDto(tuple.Card, tuple.Member, dungeon.Path, index))
                .ToList();

            var rewardPartyMembers = members
                .Select(ToRewardPartyMember)
                .ToList();

            var gamerTag = playerGroup.First().Player.User.GamerTag;
            var userId = playerGroup.First().Player.UserId;

            playerRewards.Add(new PlayerReward(playerId, userId, gamerTag, rewardCards, rewardPartyMembers));
        }

        var roomRewardType = save.CurrentRoom?.Reward ?? RewardType.None;
        var normalizedRewardType =roomRewardType;
        var blessings = await blessingRepository.GetByTypeAsync(normalizedRewardType, cancellationToken);

        var blessingRng = new Random(baseSeed);
        var blessingRewards = PickBlessings(blessings, BlessingCount, blessingRng);

        return new GetRoomRewardsResult(playerRewards, blessingRewards);
    }

    /// <summary>
    /// Selects up to <paramref name="count"/> unique cards using weighted random selection
    /// where the weight of each card is 1 / 2^(rarity-1), so higher rarity cards are less likely.
    /// </summary>
    private static List<T> PickCards<T>(List<T> pool, int count, Random rng) where T : notnull
    {
        var available = pool.ToList();
        var result = new List<T>();

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            var item = WeightedRandom(available, GetWeight, rng);
            result.Add(item);
            available.Remove(item);
        }

        return result;

        static double GetWeight(T item) =>
            item switch
            {
                Card card => 1.0 / Math.Pow(2, card.Rarity - 1),
                ValueTuple<Domain.PartyMembers.PartyMember, Card> candidate => 1.0 / Math.Pow(2, candidate.Item2.Rarity - 1),
                _ => throw new InvalidOperationException("Unsupported card pool item.")
            };
    }

    /// <summary>
    /// Selects up to <paramref name="count"/> unique blessings uniformly at random, then
    /// assigns each a rarity (1–<see cref="MaxBlessingRarity"/>) using weighted random
    /// selection where higher rarity is less likely.
    /// </summary>
    private static List<BlessingReward> PickBlessings(IList<Blessing> pool, int count, Random rng)
    {
        var available = pool.ToList();
        var result = new List<BlessingReward>();

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            var index = rng.Next(available.Count);
            var blessing = available[index];
            available.RemoveAt(index);

            int rarity = PickBlessingRarity(rng);
            string description = GetBlessingDescription(blessing, rarity);

            result.Add(new BlessingReward(blessing.Id, blessing.Type, blessing.Name, rarity, description));
        }

        return result;
    }

    /// <summary>
    /// Picks a blessing rarity from 1 to <see cref="MaxBlessingRarity"/> using
    /// <see cref="BlessingRarityWeights"/> where higher index (rarity) is less likely.
    /// </summary>
    private static int PickBlessingRarity(Random rng)
    {
        int total = BlessingRarityWeights.Sum();
        int pick = rng.Next(total);
        int cumulative = 0;
        for (int i = 0; i < BlessingRarityWeights.Length; i++)
        {
            cumulative += BlessingRarityWeights[i];
            if (pick < cumulative)
                return i + 1;
        }
        return MaxBlessingRarity;
    }

    private static string GetBlessingDescription(Blessing blessing, int rarity) =>
        rarity switch
        {
            1 => blessing.Level_1_Description,
            2 => blessing.Level_2_Description,
            3 => blessing.Level_3_Description,
            _ => blessing.Level_1_Description
        };

    private static T WeightedRandom<T>(IList<T> items, Func<T, double> weightSelector, Random rng)
    {
        double total = items.Sum(weightSelector);
        double pick = rng.NextDouble() * total;
        double cumulative = 0;
        foreach (var item in items)
        {
            cumulative += weightSelector(item);
            if (pick < cumulative)
                return item;
        }
        return items[^1];
    }

    private static CardDto ToRewardCardDto(Card card, Domain.PartyMembers.PartyMember member, string roomPath, int index)
    {
        var rewardInstanceId = GenerateDeterministicGuid($"{roomPath}|{member.Id}|{card.Id}|{index}");

        return new CardDto(
            id: 0,
            playerId: member.PlayerId,
            cardId: card.Id,
            instanceId: rewardInstanceId,
            partyMemberId: member.Id,
            classId: member.ClassId,
            color: member.Color,
            name: card.Name,
            description: card.Description,
            rarity: card.Rarity,
            isAscended: false,
            useCount: 0,
            manaCost: card.ManaCost,
            attack: card.Attack,
            magicAttack: card.MagicAttack,
            armor: card.Armor,
            magicArmor: card.MagicArmor,
            mana: card.Mana,
            strength: card.Strength,
            defense: card.Defense,
            draw: card.Draw,
            effectMultiplier: card.EffectMultiplier,
            heal: card.Heal,
            poison: card.Poison,
            lifeCost: card.LifeCost,
            playerTargets: card.PlayerTargets,
            enemyTargets: card.EnemyTargets,
            permanent: card.Permanent ?? false,
            momentum: card.Momentum,
            daggers: card.Daggers,
            spirit: card.Spirit,
            fire: card.Fire,
            water: card.Water,
            earth: card.Earth,
            air: card.Air);
    }

    private static PartyMemberReward ToRewardPartyMember(
        Domain.PartyMembers.PartyMember member) =>
        new(
            PartyMemberId: member.Id,
            Identifier: member.InstanceId,
            Color: member.Color,
            ClassId: member.ClassId,
            Gender: member.Gender,
            Health: member.Health,
            MaxHealth: member.MaxHealth,
            Mana: member.Mana,
            Speed: member.Speed,
            ClassName: member.Class.Name,
            Gold: GoldPerMember);

    private static Guid GenerateDeterministicGuid(string input)
    {
        // Stable hash-based Guid to keep reward card instance IDs reproducible for the same room path.
        // SHA-256 is used for deterministic entropy, then the first 16 bytes are used as Guid payload.
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));
        Span<byte> guidBytes = stackalloc byte[16];
        bytes.AsSpan(0, 16).CopyTo(guidBytes);
        return new Guid(guidBytes);
    }
}
