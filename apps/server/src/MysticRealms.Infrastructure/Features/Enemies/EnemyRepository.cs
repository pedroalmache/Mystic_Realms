using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Enemies;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.Enemies;

public class EnemyRepository : IEnemyRepository
{
    private readonly ApplicationDbContext _context;

    public EnemyRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IList<Enemy>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default)
    {
        return await _context.Enemies
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);
    }
}
