using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Decks;
using MysticRealms.Domain.PartyMembers;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Decks;

public class DeckRepository(ApplicationDbContext context) : IDeckRepository
{
    public async Task AddRangeAsync(IEnumerable<Deck> deck, CancellationToken cancellationToken = default)
    {
        context.Decks.AddRange(deck);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Deck>> GetByPlayerIdAsync(int playerId, CancellationToken cancellationToken = default)
    {
        return await context.Decks
            .Include(d => d.Card)
            .Where(w => w.PlayerId == playerId)
            .OrderBy(d => d.Card.Name)
            .ThenBy(d => d.Id)
            .ToListAsync(cancellationToken);
    }
}