using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.NextRoom;

public record NextRoomCommand(int GameId, string UserId, string Option, string GamerTag) : IRequest<List<GameAction>>;

public static class RoomOptions
{
    public const string Left = "L";
    public const string Right = "R";
    public const string Forward = "F";
    public const string Initial = Forward;
}
