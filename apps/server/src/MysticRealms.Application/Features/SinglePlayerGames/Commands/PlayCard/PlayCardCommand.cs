using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.PlayCard;

public record PlayCardCommand(
    int GameId,
    string UserId,
    Guid CardInstanceId,
    List<Guid> PartyMemberInstanceIds,
    List<Guid> EnemyInstanceIds) : IRequest<List<GameAction>>;
