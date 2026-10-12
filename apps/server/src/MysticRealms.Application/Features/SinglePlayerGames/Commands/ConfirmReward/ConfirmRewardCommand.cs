using MediatR;

namespace MysticRealms.Application.Features.SinglePlayerGames.Commands.ConfirmReward;

public record ConfirmRewardCardSelectionDto(int PartyMemberId, int CardId);

public record ConfirmRewardCommand(
    int GameId,
    string UserId,
    IReadOnlyList<ConfirmRewardCardSelectionDto> Cards) : IRequest;
