namespace DndCharacterSheet;

internal sealed class Character
{
    public string Name { get; private set; }
    public string Race { get; private set; }
    public string CharacterClass { get; private set; }
    
    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }
    
    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    
    internal Character() { }

    internal Character(string name, string race, string characterClass)
    {
        this.Name = name;
        this.Race = race;
        this.CharacterClass = characterClass;
    }

    internal int CalculateModifier(int statValue)
    {
        return (int)Math.Floor((statValue - 10) / 2.0);
    }
}