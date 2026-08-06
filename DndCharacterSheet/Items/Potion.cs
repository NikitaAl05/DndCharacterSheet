namespace DndCharacterSheet;

using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
internal enum PotionEffectType
{
    Heal,           
    GreaterHeal,    
    SuperiorHeal,   
    SupremeHeal,    
    Invisibility,   
    Speed,          
    Flying,
    Resistance,
    GiantStrength,
    MindReading,
    WaterBreathing,
    HealthElixir,
    Clairvoyance,
    Growth,
    Diminution,
    Poison
}

internal class Potion : Item, IHasQuantity
{
    public PotionEffectType EffectType { get; init; }
    public int Quantity { get; set; }
    
    [JsonConstructor]
    internal Potion() : base()
    {
    }

    public Potion(PotionEffectType effectType, double weight, int quantity) 
        : base(effectType.ToRussian(), weight)
    {
        this.EffectType = effectType;
        this.Quantity = quantity;
    }
}