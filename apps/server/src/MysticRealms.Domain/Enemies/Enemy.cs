namespace MysticRealms.Domain.Enemies;

public class Enemy
{
    public int Id { get; private set; }
    public Guid Guid { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public int MinHitPoints { get; private set; }
    public int MaxHitPoints { get; private set; }
    public int MinSpeed { get; private set; }
    public int MaxSpeed { get; private set; }

    private Enemy() { }

    public static Enemy Create(int id, string name, int minHitPoints, int maxHitPoints, int minSpeed, int maxSpeed)
    {
        return new Enemy
        {
            Id = id,
            Guid = Guid.NewGuid(),
            Name = name,
            MinHitPoints = minHitPoints,
            MaxHitPoints = maxHitPoints,
            MinSpeed = minSpeed,
            MaxSpeed = maxSpeed
        };
    }
}
