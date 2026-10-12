using MediatR;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.Characters.Queries.GetAllCharacters;

public class GetAllCharactersQueryHandler(
    ICharacterRepository characterRepository,
    IUserCharacterRepository userCharacterRepository ) : IRequestHandler<GetAllCharactersQuery, List<CharacterResult>>
{

    public async Task<List<CharacterResult>> Handle(GetAllCharactersQuery request, CancellationToken cancellationToken)
    {
        var characters = await characterRepository.GetAllAsync(cancellationToken);
        var userCharacters = await userCharacterRepository.Get(request.UserId, cancellationToken);
        
        var result = characters
            .OrderBy(c => c.Order)
            .Select(c => new CharacterResult(
                c.Id, 
                c.Name, 
                c.Description, 
                c.HitPoints, 
                c.Speed, 
                c.Energy, 
                userCharacters.FirstOrDefault(u => u.CharacterId == c.Id) != null && userCharacters.FirstOrDefault(u => u.CharacterId == c.Id)!.IsUnlocked))
            .ToList();
        
        return result;
    }
}
