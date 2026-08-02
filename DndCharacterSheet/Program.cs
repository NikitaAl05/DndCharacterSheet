namespace DndCharacterSheet;

class Program
{
    static void Main(string[] args)
    {
        var build = new CharacterCreationBuild();
        var character = build.Run();
        if (character != null)
        {
            var session = new CharacterSession(character);
            session.Run();
        }
        else
        {
            Console.WriteLine("\nСоздание персонажа отменено.");
        }
    }
    
}