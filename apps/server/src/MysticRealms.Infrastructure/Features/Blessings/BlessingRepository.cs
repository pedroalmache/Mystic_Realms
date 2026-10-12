using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Blessings;
using MysticRealms.Domain.Dungeons;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Blessings;

public class BlessingRepository : IBlessingRepository
{
    private readonly ApplicationDbContext _context;

    public BlessingRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Blessing>> GetByTypeAsync(
        RewardType type,
        CancellationToken cancellationToken = default)
    {
        return await _context.Blessings
            .Where(b => b.Type == type)
            .OrderBy(b => b.Id)
            .ToListAsync(cancellationToken);
    }
}
