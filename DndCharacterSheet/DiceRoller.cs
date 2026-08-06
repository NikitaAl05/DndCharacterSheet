namespace DndCharacterSheet;
using System.Text.Json.Serialization;


[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum DiceType
{
    D4 = 4,
    D6 = 6,
    D8 = 8,
    D10 = 10,
    D12 = 12,
    D20 = 20
}

internal sealed class DiceRoller
{
    private readonly Random random = new Random();

    internal int Roll(DiceType diceType, int modifier = 0)
    {
        int maxSides = (int)diceType;
        int rollResult = this.random.Next(1, maxSides + 1);
        return rollResult + modifier;
    }
}