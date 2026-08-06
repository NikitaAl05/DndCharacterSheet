namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        
        var mainMenu = new MainMenu();
        Character? character = mainMenu.Run();

        if (character != null)
        {
            var session = new CharacterSession(character);
            session.Run();
        }
        else
        {
            Console.WriteLine("\nСоздание или загрузка персонажа отменена.");
        }
    }
    
}