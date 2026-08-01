namespace DndCharacterSheet;

internal enum DamageType
{
    Bludgeoning,
    Cold,
    Fire,
    Lightning,
    Piercing,
    Poison,
    Slashing,
}

internal class Weapon : Item
{
    public DamageType Damage { get; init; }

    internal Weapon(string name, double weight, DamageType damage) : base(name, weight)
    {
        this.Damage = damage;
    }
}