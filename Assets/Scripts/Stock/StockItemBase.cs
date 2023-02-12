using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class StockItemBase : IEquatable<StockItemBase>
{
    public ItemData ItemData;
    public ItemQuality ItemQuality;
    public ItemRarity ItemRarity;

    protected StockItemBase(ItemData itemData, ItemQuality itemQuality, ItemRarity itemRarity)
    {
        ItemData = itemData;
        ItemQuality = itemQuality;
        ItemRarity = itemRarity;
    }

    public bool Equals(StockItemBase other)
    {
        if (other == null)
        {
            return false;
        }

        return
                other.ItemData.Identification == this.ItemData.Identification
                && other.ItemQuality == this.ItemQuality
                && other.ItemRarity == this.ItemRarity;
    }

    public override bool Equals(Object obj)
    {
        if (obj == null)
            return false;

        StockItem stockItemObj = obj as StockItem;
        if (stockItemObj == null)
            return false;
        else
            return Equals(stockItemObj);
    }

    public static bool operator ==(StockItemBase stockItem1, StockItemBase stockItem2)
    {
        if (((object)stockItem1) == null || ((object)stockItem2) == null)
            return Equals(stockItem1, stockItem2);

        return stockItem1.Equals(stockItem2);
    }

    public static bool operator !=(StockItemBase stockItem1, StockItemBase stockItem2)
    {
        if (((object)stockItem1) == null || ((object)stockItem2) == null)
            return !Equals(stockItem1, stockItem2);

        return !stockItem1.Equals(stockItem2);
    }


    public override int GetHashCode()
    {
        return ItemData.Identification.GetHashCode() ^ ItemQuality.GetHashCode() ^ ItemRarity.GetHashCode();
    }

    public override string ToString()
    {
        return $"Identification: {ItemData.Identification}, Quality: {ItemQuality}, Rarity: {ItemRarity}";
    }
}
