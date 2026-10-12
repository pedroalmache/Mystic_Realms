using MediatR;
using MysticRealms.Application.Interfaces;

namespace MysticRealms.Application.Features.UserCharacters.Commands;

public class CreateUserCharactersCommandHandler(
    IUserCharacterRepository userCharacterRepository): IRequestHandler<CreateUserCharactersCommand>
{
    public async Task Handle(CreateUserCharactersCommand request, CancellationToken cancellationToken)
    {
        await userCharacterRepository.AddAsync(request.UserId, 1); //warrior
        await userCharacterRepository.AddAsync(request.UserId, 2); //assassin
        await userCharacterRepository.AddAsync(request.UserId, 7); //elementalist
        await userCharacterRepository.AddAsync(request.UserId, 8); //druid
    }
}