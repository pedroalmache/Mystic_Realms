using MysticRealms.Domain.Enums;

namespace MysticRealms.ServerApi.Controllers.SinglePlayer.Models;

public class CreateSinglePlayerGameRequest
{
    public string? Seed { get; set; }
    public List<CreateSinglePlayerPartyMemberRequest> PartyMembers { get; set; }
}

public class CreateSinglePlayerPartyMemberRequest
{
    public int ClassId { get; set; }
    public string Color { get; set; }
    public Gender Gender { get; set; }
}