namespace DndCharacterSheet;

using System.Text.Json.Serialization;
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
    
    public int Gold { get; set; } = 0;

    [JsonInclude] public int CurrentHealth { get; private set; }

    [JsonInclude] public int MaxHealth { get; private set; }

    [JsonInclude] public int Strength { get; private set; }

    [JsonInclude] public int Dexterity { get; private set; }

    [JsonInclude] public int Constitution { get; private set; }

    [JsonInclude] public int Intelligence { get; private set; }

    [JsonInclude] public int Wisdom { get; private set; }

    [JsonInclude] public int Charisma { get; private set; }

    [JsonInclude] internal Inventory Inventory { get; private set; }

    [JsonInclude] internal int Level { get; private set; } = 1;

    internal Armor? BodyArmor { get; set; }
    internal Armor? Shield { get; set; }
    internal int ProficiencyBonus
    {
        get
        {
            return Level switch
            {
                <= 4 => 2,
                <= 8 => 3,
                <= 12 => 4,
                <= 16 => 5,
                <= 20 => 6,
                _ => throw new ArgumentOutOfRangeException("Max 20 lvl")
            };
        }
    }
    
    internal int ArmorClass
    {
        get
        {
            int dexMod = CalculateModifier(Dexterity);
            int totalAc = 10 + dexMod;
            
            if (BodyArmor != null)
            {
                totalAc = BodyArmor.ArmorType switch
                {
                    ArmorType.Light => BodyArmor.ArmorClassBonus + dexMod,
                    ArmorType.Medium => BodyArmor.ArmorClassBonus + Math.Min(dexMod, 2),
                    ArmorType.Heavy => BodyArmor.ArmorClassBonus,                       
                    _ => totalAc
                };
            }
            
            if (Shield != null && Shield.ArmorType == ArmorType.Shield)
            {
                totalAc += Shield.ArmorClassBonus;
            }

            return totalAc;
        }
    }

    internal Character(string name, CharacterRace characterRace, CharacterClass characterClass)
    {
        this.Name = name;
        this.CharacterRace = characterRace;
        this.CharacterClass = characterClass;
    }

    public Character()
    {
    }

    internal static int CalculateModifier(int statValue)
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

        this.MaxHealth = GetBaseHealth() + CalculateModifier(this.Constitution);
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
        int modifier = CalculateModifier(statValue);

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

    private int GetHitDieAverage()
    {
        return this.CharacterClass switch
        {
            CharacterClass.Barbarian => 7,
            CharacterClass.Fighter or CharacterClass.Paladin or CharacterClass.Ranger => 6,
            CharacterClass.Bard or CharacterClass.Cleric or CharacterClass.Druid or
                CharacterClass.Monk or CharacterClass.Rogue or CharacterClass.Warlock or
                CharacterClass.Artificer => 5,
            CharacterClass.Wizard or CharacterClass.Sorcerer => 4,
            _ => 5
        };
    }

    internal void LevelUp()
    {
        this.Level++;
        int average = this.GetHitDieAverage() + CalculateModifier(this.Constitution);
        MaxHealth += average;
        CurrentHealth += average;
    }
}