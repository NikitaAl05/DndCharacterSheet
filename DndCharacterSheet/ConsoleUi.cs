namespace DndCharacterSheet;

internal static class ConsoleUi
{
    internal static void DisplaySheet(Character character)
    {
        Console.Clear();
        DrawHeader(character);
        DrawHealthAndStats(character);
        DrawAttributes(character);
    }

    private static void DrawHeader(Character character)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        
        Console.Write("║ ");
        Console.Write(character.Name.PadRight(30));
        string raceClassStr = $"[ {character.CharacterRace.ToRussian()} {character.CharacterClass.ToRussian()} - Ур. {character.Level} ]";
        Console.Write(raceClassStr.PadLeft(27));
        Console.WriteLine(" ║");
    }

    private static void DrawHealthAndStats(Character character)
    {
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("╠═══════════════════════════════════════════════════════════╣");
        int maxBars = 12;
        double healthPercent = (double)character.CurrentHealth / character.MaxHealth;
        if (healthPercent < 0) healthPercent = 0;
        if (healthPercent > 1) healthPercent = 1;
        
        int filledBars = (int)(maxBars * healthPercent);
        string bar = new string('■', filledBars) + new string('□', maxBars - filledBars);
        
        Console.Write("║ HP: [");
        Console.Write(bar);
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.Write($"] {character.CurrentHealth}/{character.MaxHealth}".PadRight(14));
        Console.Write(" КД: 0     Бонус: +0       ║\n");
    }

    private static void DrawAttributes(Character character)
    {
        
    }
}