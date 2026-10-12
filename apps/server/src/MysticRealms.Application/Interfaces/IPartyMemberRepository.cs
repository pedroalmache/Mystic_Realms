using MysticRealms.Domain.PartyMembers;

namespace MysticRealms.Application.Interfaces;

public interface IPartyMemberRepository
{
    Task AddRangeAsync(IEnumerable<PartyMember> partyMembers, CancellationToken cancellationToken = default);
    Task<IList<PartyMember>> GetByGameIdAsync(int gameId, CancellationToken cancellationToken = default);
    Task UpdateRangeAsync(IEnumerable<PartyMember> partyMembers, CancellationToken cancellationToken = default);
}
