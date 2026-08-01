namespace DndCharacterSheet;

enum CharacterRace
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

enum CharacterClass
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
    
    internal Character() { }

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

        this.MaxHealth = 10 + this.CalculateModifier(this.Constitution);
        this.CurrentHealth = this.MaxHealth;
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

    public string GetCharacterRaceInRussian()
    {
        return this.CharacterRace switch
        {
            CharacterRace.Human => "Человек",
            CharacterRace.Elf => "Эльф",
            CharacterRace.Dwarf => "Дварф",
            CharacterRace.Halfling => "Полурослик",
            CharacterRace.Dragonborn => "Драконорожденный",
            CharacterRace.Gnome => "Гном",
            CharacterRace.HalfElf => "Полуэльф",
            CharacterRace.HalfOrc => "Полуорк",
            CharacterRace.Tiefling => "Тифлинг",
            CharacterRace.Aasimar => "Аасимар",
            CharacterRace.Genasi => "Генази",
            CharacterRace.Goliath => "Голиаф",
            CharacterRace.Tabaxi => "Табакси",
            _ => "Неизвестная раса"
        };
    }
    
    public string GetCharacterClassInRussian()
    {
        return this.CharacterClass switch
        {
            CharacterClass.Rogue => "Плут",
            CharacterClass.Ranger => "Следопыт",
            CharacterClass.Bard => "Бард",
            CharacterClass.Druid => "Друид",
            CharacterClass.Fighter => "Воин",
            CharacterClass.Wizard => "Волшебник",
            CharacterClass.Paladin => "Паладин",
            CharacterClass.Artificer => "Изобретатель",
            CharacterClass.Barbarian => "Варвар",
            CharacterClass.Cleric => "Жрец",
            CharacterClass.Monk => "Монах",
            CharacterClass.Sorcerer => "Чародей",
            CharacterClass.Warlock => "Колдун",
            _ => "Неизвестный класс"
        };
    }
}