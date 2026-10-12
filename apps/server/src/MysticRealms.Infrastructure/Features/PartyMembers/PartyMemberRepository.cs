using Microsoft.EntityFrameworkCore;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.PartyMembers;
using MysticRealms.Infrastructure.Persistence;

namespace MysticRealms.Infrastructure.Features.PartyMembers;

public class PartyMemberRepository : IPartyMemberRepository
{
    private readonly ApplicationDbContext _context;

    public PartyMemberRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddRangeAsync(IEnumerable<PartyMember> partyMembers, CancellationToken cancellationToken = default)
    {
        _context.PartyMembers.AddRange(partyMembers);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IList<PartyMember>> GetByGameIdAsync(int gameId, CancellationToken cancellationToken = default)
    {
        return await _context.PartyMembers
            .Include(pm => pm.Player)
                .ThenInclude(p => p.User)
            .Include(pm => pm.Class)
            .Where(pm => pm.Player.GameId == gameId)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateRangeAsync(IEnumerable<PartyMember> partyMembers, CancellationToken cancellationToken = default)
    {
        _context.PartyMembers.UpdateRange(partyMembers);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
