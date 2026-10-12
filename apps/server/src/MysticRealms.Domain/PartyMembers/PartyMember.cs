using MysticRealms.Domain.CharacterClasses;
using MysticRealms.Domain.Enums;
using MysticRealms.Domain.Players;

namespace MysticRealms.Domain.PartyMembers;

public class PartyMember
{
    public int Id { get; private set; }
    public Guid InstanceId { get; private set; }
    public int PlayerId { get; private set; }
    public int ClassId { get; private set; }
    public string Color { get; private set; }
    public Gender Gender { get; private set; }

    public int Health { get; private set; }
    public int MaxHealth { get; private set; }
    public int Mana { get; private set; }
    public int Speed { get; private set; }
    public int Gold { get; private set; }

    public Player Player { get; set; }
    public CharacterClass Class { get; set; }

    //Druid
    public Spirit? Spirit { get; private set; } = Enums.Spirit.None;

    //Elementalist
    public bool Fire { get; private set; }
    public bool Water { get; private set; }
    public bool Earth { get; private set; }
    public bool Air { get; private set; }

    private PartyMember() { }

    public static PartyMember Create(int playerId, int classId, string color, Gender gender,
        int hitPoints, int speed, int energy)
    {
        return new PartyMember
        {
            InstanceId = Guid.NewGuid(),
            PlayerId = playerId,
            ClassId = classId,
            Color = color,
            Gender = gender,
            MaxHealth = hitPoints,
            Health = hitPoints,
            Speed = speed,
            Mana = energy
        };
    }

    public void AddGold(int amount)
    {
        if (amount < 0)
            throw new ArgumentOutOfRangeException(nameof(amount), amount, "Gold amount must be non-negative.");

        Gold += amount;
    }
}
