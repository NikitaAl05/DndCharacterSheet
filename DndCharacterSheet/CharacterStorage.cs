namespace DndCharacterSheet;

using System.IO;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Serialization;
internal static class CharacterStorage
{
    internal static void Save(Character character, string filePath)
    {
        var options = new JsonSerializerOptions 
        { 
            WriteIndented = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            Converters = { new JsonStringEnumConverter() }
        };
        
        string jsonString = JsonSerializer.Serialize(character, options);
        File.WriteAllText(filePath, jsonString);
    }

    internal static Character Load(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return null;
        }
    
        string jsonString = File.ReadAllText(filePath);
    
        var options = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() }
        };
        
        Character character = JsonSerializer.Deserialize<Character>(jsonString, options);

        return character;
    }
}