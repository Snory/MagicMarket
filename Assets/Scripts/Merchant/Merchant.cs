using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Merchant 
{
    public MerchantData MerchantData;
    public List<StockItem> StockItems;
    public List<StockItemMarketKnowledge> ItemMarketKnowledge;
    public float CurrentGeneralMarketKnowledge;

    public StockItemMarketKnowledge GetItemMarketKnowledge(StockItemBase stockItem)
    {
        return ItemMarketKnowledge.Where(imk => imk == stockItem).FirstOrDefault();
    }

}

