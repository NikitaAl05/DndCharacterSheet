namespace DndCharacterSheet;

internal class Inventory
{
    private readonly List<Item> items = new();
    public event Action<Item> OnItemAddFailed;
    
    public double MaxWeight {get; init; }

    internal Inventory(double maxWeight)
    {
        this.MaxWeight = maxWeight;
        
    }
    
    private double GetItemWeight(Item item)
    {
        return item is IHasQuantity itemWithQty ? item.Weight * itemWithQty.Quantity : item.Weight;
    }
    
    public void AddItem(Item item)
    {
        if (this.GetTotalWeight() + GetItemWeight(item) <= this.MaxWeight)
        {
            this.items.Add(item);
        }
        else
        {
            OnItemAddFailed?.Invoke(item);
        }
    }

    internal double GetTotalWeight()
    {
        return this.items.Sum(i => this.GetItemWeight(i));
    }
    
}