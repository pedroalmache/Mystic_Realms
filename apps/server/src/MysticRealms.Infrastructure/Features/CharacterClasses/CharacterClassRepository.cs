using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.CharacterClasses;

public class CharacterClassRepository : ICharacterRepository
{
    private readonly ApplicationDbContext _context;

    public CharacterClassRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<CharacterClass>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.CharacterClasses
            .OrderBy(c => c.Id)
            .ToListAsync(cancellationToken);
    }
}
