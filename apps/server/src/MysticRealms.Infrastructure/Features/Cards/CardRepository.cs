using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Cards;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Cards;

public class CardRepository : ICardRepository
{
    private readonly ApplicationDbContext _context;

    public CardRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Card>> GetByClassIdAsync(
        int classId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Cards
            .Where(c => c.CharacterId == classId)
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }

    public async Task<IList<Card>> GetByIdsAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default)
    {
        var idList = ids.ToList();
        return await _context.Cards
            .Where(c => idList.Contains(c.Id))
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }
}
