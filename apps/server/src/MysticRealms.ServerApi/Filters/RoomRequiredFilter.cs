using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.ServerApi.Extensions;

namespace MysticRealms.ServerApi.Filters;

/// <summary>
/// Extends <see cref="GameRequiredFilter"/> by additionally requiring that the game has an
/// active combat room with a valid, parseable save state.
/// Returns 401 if unauthenticated, 404 if the game is not found or the user is not a player,
/// 400 if there is no active room or the save cannot be loaded.
/// </summary>
public class RoomRequiredFilter(IGameRepository gameRepository) : IAsyncActionFilter
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

        if (string.IsNullOrEmpty(game.Save))
        {
            context.Result = new BadRequestObjectResult(new { message = "No active combat room." });
            return;
        }

        if (SavedGameDto.FromJson(game.Save) is null)
        {
            context.Result = new BadRequestObjectResult(new { message = "Failed to load game save." });
            return;
        }

        await next();
    }
}
