namespace DndCharacterSheet;

internal static class EnumExtensions
{
    public static string ToRussian(this CharacterRace race)
    {
        return race switch
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
    
    public static string ToRussian(this CharacterClass characterClass)
    {
        return characterClass switch
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