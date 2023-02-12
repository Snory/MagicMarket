using System;

[Serializable]
public class StockItem : StockItemBase
{
    public float Amount;
    public float UnitTradePower;
    public float TotalTradePower;


    public StockItem(ItemData data, ItemQuality itemQuality, ItemRarity itemRarity, float amount, float unitTradePower, float totalTradePower) : base(data, itemQuality, itemRarity)
    {
        Amount = amount;
        UnitTradePower = unitTradePower;
        TotalTradePower = totalTradePower;
    }

    public StockItem(StockItem item) : base(item.ItemData, item.ItemQuality, item.ItemRarity)
    {
        Amount = item.Amount;
        UnitTradePower = item.UnitTradePower;
        TotalTradePower = item.TotalTradePower;
    }
}