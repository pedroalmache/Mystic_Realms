using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MysticRealms.Application.Features.Characters.Queries.GetAllCharacters;
using MysticRealms.Application.Features.Characters.Queries.GetCardsByCharacter;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;
using MysticRealms.ServerApi.Extensions;

namespace MysticRealms.ServerApi.Controllers.Characters;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class CharactersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CharactersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<List<CharacterResult>>> GetAll(CancellationToken cancellationToken)
    {
        var userId = HttpContext.GetUserId();
        var result = await _mediator.Send(new GetAllCharactersQuery(userId), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}/cards")]
    public async Task<ActionResult<List<CardRecord>>> GetCards(int id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCardsByCharacterQuery(id), cancellationToken);
        return Ok(result);
    }
}
