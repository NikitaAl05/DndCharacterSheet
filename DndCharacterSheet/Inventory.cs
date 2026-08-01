namespace DndCharacterSheet;

internal class Inventory
{
    private readonly List<Item> items = new();
    
    internal event Action<Item> OnItemAddFailed;
    internal double MaxWeight { get; init; }

    internal Item this[int index]
    {
        get => this.items[index];
    }
    internal Inventory(double maxWeight)
    {
        this.MaxWeight = maxWeight;
    }
    
    private double GetItemWeight(Item item)
    {
        return item is IHasQuantity itemWithQty ? item.Weight * itemWithQty.Quantity : item.Weight;
    }
    
    internal void AddItem(Item item)
    {
        if (this.GetTotalWeight() + this.GetItemWeight(item) <= this.MaxWeight)
        {
            this.items.Add(item);
        }
        else
        {
            this.OnItemAddFailed?.Invoke(item);
        }
    }

    internal double GetTotalWeight()
    {
        return this.items.Sum(i => this.GetItemWeight(i));
    }
}