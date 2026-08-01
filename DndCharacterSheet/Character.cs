namespace DndCharacterSheet;

internal enum CharacterRace
{
    Human,
    Elf,
    Dwarf,
    Halfling,
    Dragonborn,
    Gnome,
    HalfElf,
    HalfOrc,
    Tiefling,
    Aasimar,
    Genasi,
    Goliath,
    Tabaxi
}

internal enum CharacterClass
{
    Rogue,
    Ranger,
    Bard,
    Druid,
    Fighter,
    Wizard,
    Paladin,
    Artificer,
    Barbarian,
    Cleric,
    Monk,
    Sorcerer,
    Warlock,
}


internal sealed class Character
{
    public string Name { get; private set; }
    public CharacterRace CharacterRace { get; private set; }
    public CharacterClass CharacterClass { get; private set; }

    public int CurrentHealth { get; private set; }
    public int MaxHealth { get; private set; }

    public int Strength { get; private set; }
    public int Dexterity { get; private set; }
    public int Constitution { get; private set; }
    public int Intelligence { get; private set; }
    public int Wisdom { get; private set; }
    public int Charisma { get; private set; }
    
    internal Inventory Inventory { get; private set; }
    
    internal Character(string name, CharacterRace characterRace, CharacterClass characterClass)
    {
        this.Name = name;
        this.CharacterRace = characterRace;
        this.CharacterClass = characterClass;
    }

    internal int CalculateModifier(int statValue)
    {
        return (int)Math.Floor((statValue - 10) / 2.0);
    }

    internal void SetStats(int strength, int dexterity, int constitution, int intelligence, int wisdom, int charisma)
    {
        this.Strength = strength;
        this.Dexterity = dexterity;
        this.Constitution = constitution;
        this.Intelligence = intelligence;
        this.Wisdom = wisdom;
        this.Charisma = charisma;

        this.MaxHealth = GetBaseHealth() + this.CalculateModifier(this.Constitution);
        this.CurrentHealth = this.MaxHealth;
        
        this.Inventory = new Inventory(this.Strength * 7.5);
    }

    internal void TakeDamage(int damage)
    {
        this.CurrentHealth -= damage;
        if (this.CurrentHealth <= 0)
        {
            this.CurrentHealth = 0;
        }
    }

    internal void Heal(int amount)
    {
        this.CurrentHealth += amount;
        if (this.CurrentHealth > this.MaxHealth)
        {
            this.CurrentHealth = this.MaxHealth;
        }
    }

    private int GetBaseHealth()
    {
        return this.CharacterClass switch
        {
            CharacterClass.Barbarian => 12,
            CharacterClass.Fighter or CharacterClass.Paladin or CharacterClass.Ranger => 10,
            CharacterClass.Bard or CharacterClass.Cleric or CharacterClass.Druid or 
            CharacterClass.Monk or CharacterClass.Rogue or CharacterClass.Warlock or
            CharacterClass.Artificer => 8,     
            CharacterClass.Wizard or CharacterClass.Sorcerer => 6,
            _ => 8
        };
    }

    internal int RollAbilityCheck(DiceType diceType, DiceRoller roller, int statValue)
    {
        int modifier = this.CalculateModifier(statValue);
        
        return roller.Roll(diceType, modifier);
    }

    internal void PickUpItem(Item item)
    {
        this.Inventory.AddItem(item);
    }
    
    internal void DropItem(int index)
    {
        this.Inventory.RemoveItemAt(index);
    }

}