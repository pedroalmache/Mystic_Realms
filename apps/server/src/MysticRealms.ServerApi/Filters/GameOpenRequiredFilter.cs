using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Interfaces;
using MysticRealms.Domain.Enums;
using MysticRealms.Domain.Games;
using MysticRealms.ServerApi.Extensions;

namespace MysticRealms.ServerApi.Filters;

/// <summary>
/// Validates that the game identified by the {gameId} route value is in <see cref="GameStatus.Open"/> status.
/// When the game exists, the requesting user is a participant, and the status is anything other than
/// <see cref="GameStatus.Open"/>, the filter short-circuits and returns <c>200 OK</c> with a single-element
/// <see cref="GameAction"/> list containing a <see cref="GameActionType.GameOver"/> action.
/// In all other cases (game not found, user not authenticated, user not a participant) the request is
/// passed through to the next filter or action, allowing the existing <see cref="GameRequiredFilter"/>
/// or action logic to handle those scenarios.
/// </summary>
public class GameOpenRequiredFilter(IGameRepository gameRepository) : IAsyncActionFilter
{
    private static readonly List<GameAction> GameOverResponse = [new(GameActionType.GameOver)];
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.RouteData.Values.TryGetValue("gameId", out var gameIdObj) ||
            !int.TryParse(gameIdObj?.ToString(), out var gameId))
        {
            await next();
            return;
        }

        var userId = context.HttpContext.GetUserId();
        if (userId is null)
        {
            await next();
            return;
        }

        var game = await gameRepository.GetByIdAsync(gameId, context.HttpContext.RequestAborted);

        if (game is null || game.Players.All(p => p.UserId != userId))
        {
            await next();
            return;
        }

        if (game.GameStatus != GameStatus.Open)
        {
            context.Result = new OkObjectResult(GameOverResponse);
            return;
        }

        await next();
    }
}
