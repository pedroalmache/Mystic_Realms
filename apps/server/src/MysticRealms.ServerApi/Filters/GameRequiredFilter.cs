using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MysticRealms.Application.Interfaces;
using MysticRealms.ServerApi.Extensions;

namespace MysticRealms.ServerApi.Filters;

/// <summary>
/// Validates that the authenticated user is a participant of the game identified by the {gameId} route value.
/// Returns 401 if unauthenticated, 404 if the game is not found or the user is not a player.
/// </summary>
public class GameRequiredFilter(IGameRepository gameRepository) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("gameId", out var gameIdObj) ||
            !int.TryParse(gameIdObj?.ToString(), out var gameId))
        {
            context.Result = new NotFoundObjectResult(new { message = "Game not found." });
            return;
        }

        var userId = context.HttpContext.GetUserId();
        if (userId is null)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var game = await gameRepository.GetByIdAsync(gameId, context.HttpContext.RequestAborted);

        if (game is null || game.Players.All(p => p.UserId != userId))
        {
            context.Result = new NotFoundObjectResult(new { message = "Game not found." });
            return;
        }

        await next();
    }
}
