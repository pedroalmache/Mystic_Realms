using MediatR;

namespace MysticRealms.Application.Features.UserCharacters.Commands;

public record CreateUserCharactersCommand(string UserId): IRequest;