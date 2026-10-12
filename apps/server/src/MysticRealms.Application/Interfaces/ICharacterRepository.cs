using MysticRealms.Domain.CharacterClasses;

namespace MysticRealms.Application.Interfaces;

public interface ICharacterRepository
{
    Task<List<CharacterClass>> GetAllAsync(CancellationToken cancellationToken = default);
}
