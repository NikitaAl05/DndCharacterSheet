namespace DndCharacterSheet;

using System.Text.Json.Serialization;
internal class Inventory
{
    [JsonInclude] public List<Item> items = new();
    
    public event Action<Item> OnItemAddFailed;
    [JsonInclude] public double MaxWeight { get; init; }
    
    public Item this[int index]
    {
        get => this.items[index];
    }
    
    [JsonConstructor]
    internal Inventory()
    {
    }
    
    public Inventory(double maxWeight)
    {
        this.MaxWeight = maxWeight;
    }
    
    public int Count => this.items.Count;
    
    public void DecreaseItemQuantity(int index, int amountToRemove)
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
    
    public void AddItem(Item item)
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

    public double GetTotalWeight()
    {
        return this.items.Sum(i => this.GetItemWeight(i));
    }

    public void RemoveItemAt(int index)
    {
        if (index >= 0 && index < this.items.Count)
        {
            this.items.RemoveAt(index);
        }
    }
}