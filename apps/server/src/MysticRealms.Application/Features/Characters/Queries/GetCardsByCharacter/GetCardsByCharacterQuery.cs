using MediatR;
using MysticRealms.Application.Features.SinglePlayerGames.Dtos;

namespace MysticRealms.Application.Features.Characters.Queries.GetCardsByCharacter;

public record GetCardsByCharacterQuery(int CharacterId) : IRequest<List<CardRecord>>;
