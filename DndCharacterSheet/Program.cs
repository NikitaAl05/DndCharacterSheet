namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        Character hero = new Character("Никита", CharacterRace.Human, CharacterClass.Fighter);

        hero.SetStats(20,10,16,5,5,8);
        
        ConsoleUi.DisplaySheet(hero);
        
    }
}