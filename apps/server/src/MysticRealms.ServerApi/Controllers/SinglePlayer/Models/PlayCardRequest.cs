using System.ComponentModel.DataAnnotations;

namespace MysticRealms.ServerApi.Controllers.SinglePlayer.Models;

public class PlayCardRequest
{
    [Required]
    public Guid CardInstanceId { get; set; }
    public List<Guid> PartyMemberInstanceIds { get; set; } = new();
    public List<Guid> EnemyInstanceIds { get; set; } = new();
}
