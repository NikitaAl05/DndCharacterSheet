namespace DndCharacterSheet;

using System.Text.Json.Serialization;

[JsonPolymorphic]
[JsonDerivedType(typeof(Weapon), typeDiscriminator: "weapon")]
[JsonDerivedType(typeof(Armor), typeDiscriminator: "armor")]
[JsonDerivedType(typeof(Potion), typeDiscriminator: "potion")]
[JsonDerivedType(typeof(Equipment), typeDiscriminator: "equipment")]
internal abstract class Item
{
    public string Name { get; init; }
    public double Weight { get; init; }
    
    protected Item()
    {
    }
    protected Item(string name, double weight)
    {
       this.Name = name;
       this.Weight = weight;
    }
}
