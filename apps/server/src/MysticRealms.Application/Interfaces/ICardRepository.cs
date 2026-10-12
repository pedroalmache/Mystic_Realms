using MysticRealms.Domain.Cards;

namespace MysticRealms.Application.Interfaces;

public interface ICardRepository
{
    Task<IList<Card>> GetByClassIdAsync(int classId, CancellationToken cancellationToken = default);
    Task<IList<Card>> GetByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
