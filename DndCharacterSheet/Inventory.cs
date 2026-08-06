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
    
    internal int Count => this.items.Count;
    
    internal void DecreaseItemQuantity(int index, int amountToRemove)
    {
        if (index >= 0 && index < this.items.Count)
        {
            var item = this.items[index];
            if (item is IHasQuantity itemWithQty)
            {
                if (amountToRemove >= itemWithQty.Quantity)
                {
                    this.RemoveItemAt(index);
                }
                else
                {
                    itemWithQty.Quantity -= amountToRemove;
                }
            }
            else
            {
                this.RemoveItemAt(index);
            }
        }
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

    internal void RemoveItemAt(int index)
    {
        if (index >= 0 && index < this.items.Count)
        {
            this.items.RemoveAt(index);
        }
    }
}