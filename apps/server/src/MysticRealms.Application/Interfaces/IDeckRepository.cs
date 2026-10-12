using MysticRealms.Domain.Decks;

namespace MysticRealms.Application.Interfaces;

public interface IDeckRepository
{
    Task AddRangeAsync(IEnumerable<Deck> deck, CancellationToken cancellationToken = default);
    Task<List<Deck>> GetByPlayerIdAsync(int playerId, CancellationToken cancellationToken = default);
}