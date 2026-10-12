namespace MysticRealms.Domain.CharacterClasses;

public class CharacterClass
{
    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public int HitPoints { get; private set; }
    public int Speed { get; private set; }
    public int Energy { get; private set; }
    public int Order { get; private set; } = 0;

    private CharacterClass() { }
}
