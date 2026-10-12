using MediatR;
using MysticRealms.Domain.Enums;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.CreateSinglePlayerGame;

public record CreateSinglePlayerGameCommand(
    string UserId,
    string? Seed,
    List<CreateSinglePlayerPartyMemberDto> PartyMembers
) : IRequest<GameDto>;

public record CreateSinglePlayerPartyMemberDto(
    int ClassId,
    string Color,
    Gender Gender
);

public record GameDto(int Id, string Seed, int PlayerCount);
