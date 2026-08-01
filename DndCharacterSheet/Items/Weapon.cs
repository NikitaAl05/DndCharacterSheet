namespace DndCharacterSheet;

internal enum DamageType
{
    Acid,
    Bludgeoning,
    Cold,
    Fire,
    Force,
    Lightning,
    Necrotic,
    Piercing,
    Poison,
    Psychic,
    Radiant,
    Slashing,
    Thunder
}

internal sealed class Weapon : Item
{
    internal DiceType DamageDice { get; init; }
    internal int DamageBonus { get; init; }
    internal DamageType DamageType { get; init; }

    internal Weapon(string name, double weight, DiceType damageDice, int damageBonus, DamageType damageType) 
        : base(name, weight)
    {
        this.DamageDice = damageDice;
        this.DamageBonus = damageBonus;
        this.DamageType = damageType;
    }
}