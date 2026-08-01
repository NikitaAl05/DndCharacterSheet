namespace DndCharacterSheet;

internal enum ArmorType
{
    Light,
    Medium,
    Heavy,
    Shield
}

internal class Armor : Item
{
    public ArmorType ArmorType {  get; init; }
    public int ArmorClassBonus { get; init; }

    internal Armor(string name, double weight, ArmorType armor ,int armorClassBonus) : base(name, weight)
    {
        this.ArmorType = armor;
        this.ArmorClassBonus = armorClassBonus;
    }
}