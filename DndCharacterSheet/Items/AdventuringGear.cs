namespace DndCharacterSheet;

internal class AdventuringGear : Item, IHasQuantity
{
    public int Quantity { get; init; }

    public AdventuringGear(string name, double weight) : base(name, weight)
    {
        this.Quantity = 1;
    }

    public AdventuringGear(string name, double weight, int quantity) : base(name, weight)
    {
        this.Quantity = quantity;
    }
}