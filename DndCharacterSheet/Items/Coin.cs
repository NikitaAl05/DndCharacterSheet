namespace DndCharacterSheet;

internal enum CoinType
{
    Copper,
    Silver,
    Gold
}

internal class Coin : Item, IHasQuantity
{
    public CoinType CoinType { get; init; }
    public int Quantity { get; init; }

    public Coin(CoinType coinType, int quantity) 
        : base(coinType.ToRussian(), 0.02)
    {
        this.CoinType = coinType;
        this.Quantity = quantity;
    }
}