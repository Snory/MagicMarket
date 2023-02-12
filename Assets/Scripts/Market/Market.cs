using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]

public class Market 
{
    public List<StockItem> StockItems;

    /// <summary>
    /// Calculate new market stock value (weighted average) for items in transaction
    /// </summary>
    /// <param name="goals"></param>
    /// <param name="offers"></param>
    public void AddTransaction(List<TradeStockItem> goals, List<TradeStockItem> offers)
    {

        float totalGoalTradeMarketPrice = goals.Sum(g => g.MarketTotalPrice);
        float totalOfferTradeMarketPrice = offers.Sum(o => o.MarketTotalPrice);

        foreach (var goal in goals)
        {
            float marketUnitPrice = goal.MarketTotalPrice;
            float marketTotalPrice = marketUnitPrice * goal.Amount;

            float newValue = (marketTotalPrice / totalGoalTradeMarketPrice) * totalOfferTradeMarketPrice;

            AddStockItem(new StockItem
            {
                ItemData = goal.ItemData,
                ItemQuality = goal.ItemQuality,
                ItemRarity = goal.ItemRarity,
                Amount = goal.Amount,
                TotalTradePower = goal.Amount * newValue,
                UnitTradePower = newValue
            });
           
        }

        foreach (var offer in offers)
        {

            float marketUnitPrice = offer.MarketUnitPrice;
            float marketTotalPrice = marketUnitPrice * offer.Amount;

            float newValue = (marketTotalPrice / totalOfferTradeMarketPrice) * totalGoalTradeMarketPrice;
            AddStockItem(new StockItem
            {
                ItemData = offer.ItemData,
                ItemQuality = offer.ItemQuality,
                ItemRarity = offer.ItemRarity,
                Amount = offer.Amount,
                TotalTradePower = offer.Amount * newValue,
                UnitTradePower = newValue
            });
        }

    }

    private void AddStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        if(currentStockItem == null)
        {
            StockItems.Add(item);
        } else
        {
            currentStockItem.TotalTradePower += item.TotalTradePower;
            currentStockItem.Amount += item.Amount;
            currentStockItem.UnitTradePower = currentStockItem.TotalTradePower / currentStockItem.Amount;
        }
    }

    public StockItem GetStockItem(StockItem stockItem)
    {
        return StockItems.Where(si => si == stockItem).FirstOrDefault();
    }


   
}
