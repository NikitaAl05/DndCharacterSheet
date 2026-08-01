namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        Character hero = new Character("Никита", CharacterRace.Human, CharacterClass.Fighter);

        hero.SetStats(20,10,16,5,5,8);

        Console.WriteLine($"Current HP: {hero.CurrentHealth}");
        hero.TakeDamage(10);
        Console.WriteLine($"Current HP: {hero.CurrentHealth}");
        
        CharacterStorage.Save(hero, "save.json");
        
        Character loadHero = CharacterStorage.Load("save.json");
        Console.WriteLine(loadHero.CurrentHealth);
        Console.WriteLine($"Current HP: {loadHero.CurrentHealth}");

    }
}