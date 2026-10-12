using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomRewards;

public record GetRoomRewardsQuery(int GameId, string UserId) : IRequest<GetRoomRewardsResult?>;

public record GetRoomRewardsResult(
    List<PlayerReward> Players,
    List<BlessingReward> Blessings);

public record PlayerReward(
    int PlayerId,
    string UserId,
    string GamerTag,
    List<CardDto> Cards,
    List<PartyMemberReward> PartyMembers);

public record PartyMemberReward(
    int PartyMemberId,
    Guid Identifier,
    string Color,
    int ClassId,
    Gender Gender,
    int Health,
    int MaxHealth,
    int Mana,
    int Speed,
    string ClassName,
    int Gold);

public record BlessingReward(
    int BlessingId,
    RewardType Type,
    string Name,
    int Rarity,
    string Description);
