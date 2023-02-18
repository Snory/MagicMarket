using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Merchant : IStockable, IMarketKnowledable
{
    public MerchantData MerchantData;
    public List<StockItem> StockItems;
    public List<StockItemMarketKnowledge> ItemMarketKnowledge;
    public float TradePower;


    public void AddStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        if (currentStockItem == null)
        {
            StockItems.Add(new StockItem(item));
        }
        else
        {
            currentStockItem.TotalTradePower += item.TotalTradePower;
            currentStockItem.Amount += item.Amount;
            currentStockItem.UnitTradePower = currentStockItem.TotalTradePower / currentStockItem.Amount;
        }
    }


    public void UpdateStockItemKnowledge(StockItem stockItem, float value)
    {
        StockItemMarketKnowledge currentKnowledge = GetItemMarketKnowledge(stockItem);

        if (currentKnowledge == null)
        {
            StockItemMarketKnowledge newKnowledge = new StockItemMarketKnowledge(stockItem.ItemData, stockItem.ItemQuality, stockItem.ItemRarity, value);
            ItemMarketKnowledge.Add(newKnowledge);
        }
        else
        {
            currentKnowledge.UnitTradePower = stockItem.UnitTradePower;
        }
    }


    public StockItemMarketKnowledge GetItemMarketKnowledge(StockItemBase stockItem)
    {
        return ItemMarketKnowledge.Where(imk => imk == stockItem).FirstOrDefault();
    }

    public void RemoveStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        currentStockItem.TotalTradePower -= item.TotalTradePower;
        currentStockItem.Amount -= item.Amount;
        currentStockItem.UnitTradePower = currentStockItem.TotalTradePower / currentStockItem.Amount;


        if (currentStockItem.Amount <= 0)
        {
            StockItems.Remove(currentStockItem);
        }
    }

    public void UpdateTadePower(int value)
    {
        TradePower += value;
    }
}

