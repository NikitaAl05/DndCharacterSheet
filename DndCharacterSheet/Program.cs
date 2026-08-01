namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        Character hero = new Character("Никита", "Человек", "Воин");

        hero.SetStats(20,10,16,5,5,8);
        
        Console.WriteLine($"Персонаж: {hero.Name} ({hero.Race} {hero.CharacterClass})");
        Console.WriteLine($"Здоровье: {hero.CurrentHealth}/{hero.MaxHealth}");
        Console.WriteLine($"\nМодификатор Силы (20): +{hero.CalculateModifier(hero.Strength)} ");
        Console.WriteLine($"Модификатор Ловкости (10): {hero.CalculateModifier(hero.Dexterity)} ");
        Console.WriteLine($"Модификатор Телосложения (16): +{hero.CalculateModifier(hero.Constitution)} ");
        Console.WriteLine($"Модификатор Интелекта (5): {hero.CalculateModifier(hero.Intelligence)} ");
        Console.WriteLine($"Модификатор Мудрость (5): {hero.CalculateModifier(hero.Wisdom)} ");
        Console.WriteLine($"Модификатор Харизма (8): +{hero.CalculateModifier(hero.Strength)} ");
        
        // Тест урона
        Console.WriteLine("\nПерсонад получает 3 урона");
        hero.TakeDamage(5);
        Console.WriteLine($"Текущее здоровье: {hero.CurrentHealth}/{hero.MaxHealth}");
        
        // тест овер урона 
        Console.WriteLine("\nПерсонад получает 30 урона");
        hero.TakeDamage(30);
        Console.WriteLine($"Текущее здоровье: {hero.CurrentHealth}/{hero.MaxHealth}");
        
        //тест хила 
        Console.WriteLine("\nПерсонад лечится на 5 хп");
        hero.Heal(5);
        Console.WriteLine($"Текущее здоровье: {hero.CurrentHealth}/{hero.MaxHealth}");
        
        //тест овер хила 
        Console.WriteLine("\nПерсонад лечится на 113 хп");
        hero.Heal(113);
        Console.WriteLine($"Текущее здоровье: {hero.CurrentHealth}/{hero.MaxHealth}");
    }
}