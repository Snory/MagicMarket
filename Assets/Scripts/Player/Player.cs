using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Player 
{
    public List<StockItem> StockItems;
    public int ReputationPoints;
    public int KarmaPoints;
    public int TradingPoints;
    public int TradePower;

    public void CloseTrade(List<TradeStockItem> sold, List<TradeStockItem> bought)
    {

        float totalSoldPrice = sold.Sum(g => g.MerchantTotalPrice); //this is how merchant valued the stuff he sold
        float totalBoughtPrice = bought.Sum(o => o.MerchantTotalPrice); //this is how merchant valued the stuff he bought

        foreach (var soldItem in sold) //remove from stock excatly what i sold
        {
            float newValue = (soldItem.MerchantTotalPrice / totalSoldPrice) * totalBoughtPrice;

            RemoveStockItem(new StockItem
            (
                soldItem.ItemData,
                soldItem.ItemQuality,
                soldItem.ItemRarity,
                soldItem.Amount,
                soldItem.UnitTradePower,
                soldItem.TotalTradePower
            ));

            TradePower -= (int) soldItem.MarketTotalPrice;

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

            TradePower -= (int)boughtItem.MarketTotalPrice;
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

    private void RemoveStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        currentStockItem.TotalTradePower -= item.TotalTradePower;
        currentStockItem.Amount -= item.Amount;
        currentStockItem.UnitTradePower = currentStockItem.TotalTradePower / currentStockItem.Amount;

        if(currentStockItem.Amount <= 0)
        {
            StockItems.Remove(currentStockItem);
        }
    }
}

