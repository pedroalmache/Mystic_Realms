using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDiscardPile;

public record GetDiscardPileQuery(int GameId, string UserId) : IRequest<GetDiscardPileResult?>;

/// <summary>
/// The discard pile cards in the order they were discarded (oldest first).
/// </summary>
public record GetDiscardPileResult(IList<CardDto> Cards);
