using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetPlayerDeck;

public record GetPlayerDeckQuery(int GameId, int PlayerId, string UserId) : IRequest<GetPlayerDeckResult?>;

public record GetPlayerDeckResult(IList<CardDto> Cards);
