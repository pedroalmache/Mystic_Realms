using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomOptions;

public record GetRoomOptionsQuery(int GameId) : IRequest<GetRoomOptionsResult?>;

public record GetRoomOptionsResult(RoomDto Left, RoomDto Right);
