namespace DndCharacterSheet;

internal static class EnumExtensions
{
    internal static string ToRussian(this CharacterRace race)
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
    
    internal static string ToRussian(this CharacterClass characterClass)
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
    
    internal static string ToRussian(this PotionEffectType potionEffectType)
    {
        return potionEffectType switch
        {
            PotionEffectType.Heal => "Лечение",
            PotionEffectType.GreaterHeal => "Великое лечение",
            PotionEffectType.SuperiorHeal => "Превосходное",
            PotionEffectType.SupremeHeal => "Высшее лечение",
            PotionEffectType.Invisibility => "Невидимость",
            PotionEffectType.Speed => "Скорость",
            PotionEffectType.Flying => "Полёт",
            PotionEffectType.Resistance => "Сопротивление",
            PotionEffectType.GiantStrength => "Сила великана",
            PotionEffectType.MindReading => "Чтение мыслей",
            PotionEffectType.WaterBreathing => "Водное дыхание",
            PotionEffectType.HealthElixir => "Эликсир здоровья",
            PotionEffectType.Clairvoyance => "Ясновидение",
            PotionEffectType.Growth => "Рост",
            PotionEffectType.Diminution => "Уменьшение",
            PotionEffectType.Poison => "Яд",
            _ => "Эффект"
        };
    }

    internal static string ToRussian(this DamageType damageType)
    {
        return damageType switch
        {
            DamageType.Acid => "Кислота",
            DamageType.Bludgeoning => "Дробящий",
            DamageType.Cold => "Холод",
            DamageType.Fire => "Огонь",
            DamageType.Force => "Силовой",
            DamageType.Lightning => "Электр.",
            DamageType.Necrotic => "Некрот.",
            DamageType.Piercing => "Колющий",
            DamageType.Poison => "Яд",
            DamageType.Psychic => "Психич.",
            DamageType.Radiant => "Лучистый",
            DamageType.Slashing => "Рубящий",
            DamageType.Thunder => "Звуковой",
            _ => "Урон"
        };
    }
    
    internal static string ToRussian(this ArmorType armorType)
    {
        return armorType switch
        {
            ArmorType.Light => "Лёгкий доспех",
            ArmorType.Medium => "Средний доспех",
            ArmorType.Heavy => "Тяжёлый доспех",
            ArmorType.Shield => "Щит",
            _ => "Неизвестный тип доспеха"
        };
    }
}