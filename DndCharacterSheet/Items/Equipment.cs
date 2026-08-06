namespace DndCharacterSheet;
using System.Text.Json.Serialization;

internal sealed class Equipment : Item, IHasQuantity
{
    public int Quantity { get; set; }

    [JsonConstructor]
    internal Equipment() : base()
    {
    }
    
    public Equipment(string name, double weight) : base(name, weight)
    {
        this.Quantity = 1;
    }

    public Equipment(string name, double weight, int quantity) : base(name, weight)
    {
        this.Quantity = quantity;
    }
}