namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        Character hero = new Character("Никита", "Человек", "Воин");

        int strength = 20;
        int dexterity = 10;
        
        int strengthModifier = hero.CalculateModifier(strength);
        int dexterityModifier = hero.CalculateModifier(dexterity);
        
        Console.WriteLine($"Персонаж: {hero.Name} ({hero.Race} {hero.CharacterClass})");
        Console.WriteLine($"Сила: {strength} | Модификатор: {strengthModifier}");
        Console.WriteLine($"Ловкость: {dexterity} | Модификатор: {dexterityModifier}");
    }
}