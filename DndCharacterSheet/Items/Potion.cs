namespace DndCharacterSheet;

internal enum PotionEffectType
{
    Heal
}

internal class Potion : Item, IHasQuantity
{
    public PotionEffectType EffectType { get; init; }
    public int Quantity { get; init; }

    public Potion(PotionEffectType effectType, double weight, int quantity) 
        : base(effectType.ToRussian(), weight)
    {
        this.EffectType = effectType;
        this.Quantity = quantity;
    }
}