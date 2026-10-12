using MysticRealms.Application.Features.Characters.Queries.GetAllCharacters;
using MysticRealms.Domain.UserCharacters;

namespace MysticRealms.Application.Interfaces;

public interface IUserCharacterRepository
{
    Task<UserCharacter> AddAsync(string userId, int characterId, CancellationToken cancellationToken = default);
    Task<List<UserCharacter>> Get(string userId, CancellationToken cancellationToken = default);
}