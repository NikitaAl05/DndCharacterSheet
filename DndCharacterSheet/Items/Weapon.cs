namespace DndCharacterSheet;
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
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
    public DiceType DamageDice { get; init; }
    public int DamageBonus { get; init; }
    public DamageType DamageType { get; init; }
    
    internal Weapon() : base()
    {
    }
    
    [JsonConstructor]
    internal Weapon(string name, double weight, DiceType damageDice, int damageBonus, DamageType damageType) 
        : base(name, weight)
    {
        this.DamageDice = damageDice;
        this.DamageBonus = damageBonus;
        this.DamageType = damageType;
    }
}