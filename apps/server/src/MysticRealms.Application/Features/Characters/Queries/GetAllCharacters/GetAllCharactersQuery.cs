using MediatR;

namespace MysticRealms.Application.Features.Characters.Queries.GetAllCharacters;

public record GetAllCharactersQuery(string UserId) : IRequest<List<CharacterResult>>;

public record CharacterResult(int Id, string Name, string Description, int HitPoints, int Speed, int Energy, bool IsUnlocked);
