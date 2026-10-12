using MediatR;
using Microsoft.AspNetCore.Identity;
using MysticRealms.Application.Features.UserCharacters.Commands;
using MysticRealms.Domain.Identity;

namespace MysticRealms.Infrastructure.Services;

public class SeedService(UserManager<ApplicationUser> userManager, IMediator mediator)
{
    public async Task SeedAsync()
    {
        const string email = "pedropa67@hotmail.com";
        const string password = "P@ssw0rd!";
        const string gamerTag = "WhiteKnight";

        var existing = await userManager.FindByEmailAsync(email);
        if (existing is not null)
            return;

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            GamerTag = gamerTag
        };

        var result = await userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to seed test user: {errors}");
        }

        await mediator.Send(new CreateUserCharactersCommand(user.Id));
    }
}
