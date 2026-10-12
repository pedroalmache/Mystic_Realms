using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDrawPile;

public record GetDrawPileQuery(int GameId, string UserId) : IRequest<GetDrawPileResult?>;

/// <summary>
/// The draw pile cards ordered by card ID (ascending).
/// </summary>
public record GetDrawPileResult(IList<CardDto> Cards);
