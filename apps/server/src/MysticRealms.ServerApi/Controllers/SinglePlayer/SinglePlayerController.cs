using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.ConfirmReward;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.CreateSinglePlayerGame;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.EndTurn;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.NextRoom;
using MysticRealms.Application.Features.SinglePlayerGames.Commands.PlayCard;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActionLog;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetActiveSinglePlayerGame;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDiscardPile;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetDrawPile;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetPlayerDeck;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomOptions;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetRoomRewards;
using MysticRealms.Application.Features.SinglePlayerGames.Queries.GetSinglePlayerGame;
using MysticRealms.ServerApi.Controllers.SinglePlayer.Models;
using MysticRealms.ServerApi.Extensions;
using MysticRealms.ServerApi.Filters;

namespace MysticRealms.ServerApi.Controllers.SinglePlayer;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SinglePlayerController : ControllerBase
{
    private readonly IMediator _mediator;

    public SinglePlayerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("active")]
    public async Task<ActionResult<ActiveSinglePlayerGameResult>> GetActive(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
            return Unauthorized();

        var result = await _mediator.Send(new GetActiveSinglePlayerGameQuery(userId), cancellationToken);

        if (result is null)
            return NotFound(new { message = "No active single player game found." });

        return Ok(result);
    }

    [HttpGet("{gameId:int}")]
    [GameRequired]
    public async Task<ActionResult<GetSinglePlayerGameResult>> GetGame(int gameId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var result = await _mediator.Send(new GetSinglePlayerGameQuery(gameId, userId), cancellationToken);

        if (result is null)
            return NotFound(new { message = "Game not found." });

        return Ok(result);
    }
    
    [HttpGet("{gameId:int}/draw-pile")]
    [GameRequired]
    public async Task<ActionResult<GetDrawPileResult>> GetDrawPile(int gameId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var result = await _mediator.Send(new GetDrawPileQuery(gameId, userId), cancellationToken);

        if (result is null)
            return NotFound(new { message = "No active encounter found for this game." });

        return Ok(result);
    }

    [HttpGet("{gameId:int}/discard-pile")]
    [GameRequired]
    public async Task<ActionResult<GetDiscardPileResult>> GetDiscardPile(int gameId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var result = await _mediator.Send(new GetDiscardPileQuery(gameId, userId), cancellationToken);

        if (result is null)
            return NotFound(new { message = "No active encounter found for this game." });

        return Ok(result);
    }

    [HttpGet("{gameId:int}/players/{playerId:int}/deck")]
    [GameRequired]
    public async Task<ActionResult<GetPlayerDeckResult>> GetPlayerDeck(int gameId, int playerId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var result = await _mediator.Send(new GetPlayerDeckQuery(gameId, playerId, userId), cancellationToken);

        if (result is null)
            return NotFound(new { message = "Player deck not found for this game." });

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<GameDto>> Create([FromBody] CreateSinglePlayerGameRequest request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        if (userId is null)
            return Unauthorized();

        try
        {
            var command = new CreateSinglePlayerGameCommand(
                userId,
                request.Seed,
                request.PartyMembers.Select(x =>
                        new CreateSinglePlayerPartyMemberDto(
                            x.ClassId,
                            x.Color,
                            x.Gender))
                    .ToList()
            );
            
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{gameId:int}/next-turn")]
    [RoomRequired]
    [GameOpenRequired]
    public async Task<ActionResult<List<GameAction>>> NextTurn(int gameId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;

        try
        {
            var command = new EndTurnCommand(gameId, userId);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{gameId:int}/next-room")]
    [RoomRequired]
    [GameOpenRequired]
    public async Task<ActionResult<List<GameAction>>> NextRoom(
        int gameId,
        [FromBody] NextRoomRequest request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var gamerTag = HttpContext.GetGamerTag() ?? string.Empty;

        try
        {
            var option = request.Direction switch
            {
                NextRoomDirection.Left => RoomOptions.Left,
                NextRoomDirection.Right => RoomOptions.Right,
                NextRoomDirection.Forward => RoomOptions.Forward,
                _ => throw new ArgumentOutOfRangeException(
                    nameof(request.Direction),
                    request.Direction,
                    "Invalid room direction specified.")
            };

            var command = new NextRoomCommand(gameId, userId, option, gamerTag);
            var result = await _mediator.Send(command, cancellationToken);
            var resultEndTurn = await _mediator.Send(new EndTurnCommand(gameId, userId));
            return Ok(resultEndTurn);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("{gameId:int}/play-card")]
    [RoomRequired]
    [GameOpenRequired]
    public async Task<ActionResult<List<GameAction>>> PlayCard(int gameId, [FromBody] PlayCardRequest request, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;

        try
        {
            var command = new PlayCardCommand(
                gameId,
                userId,
                request.CardInstanceId,
                request.PartyMemberInstanceIds,
                request.EnemyInstanceIds);
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    
    [HttpGet("{gameId:int}/room-rewards")]
    [RoomRequired]
    public async Task<ActionResult<GetRoomRewardsResult>> GetRoomRewards(int gameId, CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId()!;
        var result = await _mediator.Send(new GetRoomRewardsQuery(gameId, userId), cancellationToken);

        if (result is null)
            return BadRequest(new { message = "Room rewards are only available after winning a room." });

        return Ok(result);
    }

    [HttpPost("{gameId:int}/confirm-reward")]
    [RoomRequired]
    public async Task<IActionResult> ConfirmReward(
        int gameId,
        [FromBody] List<ConfirmRewardCardRequest> request,
        CancellationToken cancellationToken)
    {
        try
        {
            var userId = HttpContext.GetUserId()!;
            await _mediator.Send(
                new ConfirmRewardCommand(
                    gameId,
                    userId,
                    request.Select(c => new ConfirmRewardCardSelectionDto(c.PartyMemberId, c.CardId)).ToList()),
                cancellationToken);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("{gameId:int}/room-options")]
    [RoomRequired]
    public async Task<ActionResult<GetRoomOptionsResult>> GetRoomOptions(int gameId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRoomOptionsQuery(gameId), cancellationToken);

        if (result is null)
            return BadRequest(new { message = "Room options are only available in a room selection room." });

        return Ok(result);
    }
}
