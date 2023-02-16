using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Merchant
{
    public MerchantData MerchantData;
    public List<StockItem> StockItems;
    public List<StockItemMarketKnowledge> ItemMarketKnowledge;
    public float TradePower;
    public float GeneralMarketKnowledge;

    public void CloseTrade(List<TradeStockItem> sold, List<TradeStockItem> bought)
    {

        float totalSoldPrice = sold.Sum(g => g.MerchantTotalPrice); //this is how merchant valued the stuff he sold
        float totalBoughtPrice = bought.Sum(o => o.MerchantTotalPrice); //this is how merchant valued the stuff he bought

        foreach (var soldItem in sold) //remove from stock excatly what i sold
        {
            float newValue = (soldItem.MerchantTotalPrice / totalSoldPrice) * totalBoughtPrice;

            UpdateStockItemKnowledge(soldItem, newValue);

            RemoveStockItem(new StockItem
            (
                soldItem.ItemData,
                soldItem.ItemQuality,
                soldItem.ItemRarity,
                soldItem.Amount,
                soldItem.UnitTradePower,
                soldItem.TotalTradePower
            ));

            TradePower -= soldItem.MarketTotalPrice;
        }

        foreach (var boughtItem in bought)
        {
            float newValue = (boughtItem.TotalTradePower / totalBoughtPrice) * totalSoldPrice;
            StockItem newItem = new StockItem
            (
                boughtItem.ItemData,
                boughtItem.ItemQuality,
                boughtItem.ItemRarity,
                boughtItem.Amount,
                newValue,
                boughtItem.Amount * newValue
            );

            AddStockItem(newItem);

            TradePower += boughtItem.MarketTotalPrice;
        }
    }

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

    public void UpdateGeneralMarketKnowledge()
    {
        float itemMarketKnowledgeTotal = 0;
        foreach (var itemMarketKnowledge in ItemMarketKnowledge)
        {
            itemMarketKnowledgeTotal += itemMarketKnowledge.UnitTradePower;
        }

        GeneralMarketKnowledge = (TradePower / itemMarketKnowledgeTotal) > 1 ? ((itemMarketKnowledgeTotal / TradePower)) : ((TradePower / itemMarketKnowledgeTotal));
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

    private void RemoveStockItem(StockItem item)
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

    public StockItemMarketKnowledge GetItemMarketKnowledge(StockItemBase stockItem)
    {
        return ItemMarketKnowledge.Where(imk => imk == stockItem).FirstOrDefault();
    }

}

