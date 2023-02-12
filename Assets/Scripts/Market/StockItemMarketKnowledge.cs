using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class StockItemMarketKnowledge : StockItemBase
{
    

    public float UnitTradePower;

    public StockItemMarketKnowledge(ItemData itemData, ItemQuality itemQuality, ItemRarity itemRarity, float unitTradePower) : base(itemData, itemQuality, itemRarity)
    {
        UnitTradePower = unitTradePower;
    }
}
