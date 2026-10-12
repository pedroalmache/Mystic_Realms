using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.EndTurn;

public record EndTurnCommand(int GameId, string UserId) : IRequest<List<GameAction>>;
