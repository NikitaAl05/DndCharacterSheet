namespace DndCharacterSheet;

internal abstract class Item
{
    public string Name { get; init; }
    public double Weight { get; init; }

    protected Item(string name, double weight)
    {
       this.Name = name;
       this.Weight = weight;
    }
}
