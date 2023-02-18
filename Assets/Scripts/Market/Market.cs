using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]

public class Market
{
    public List<StockItem> StockItems;
    public List<StockItemsTransaction> StockItemsTransactions;

    /// <summary>
    /// Calculate new market stock value (weighted average) for items in transaction
    /// </summary>
    /// <param name="goals"></param>
    /// <param name="offers"></param>
    public void CloseTrade(List<TradeStockItem> goals, List<TradeStockItem> offers)
    {

        float totalGoalTradeMarketPrice = goals.Sum(g => g.MarketTotalPrice);
        float totalOfferTradeMarketPrice = offers.Sum(o => o.MarketTotalPrice);

        foreach (var goal in goals)
        {
            float marketUnitPrice = goal.MarketTotalPrice;
            float marketTotalPrice = marketUnitPrice * goal.Amount;

            float newValue = (marketTotalPrice / totalGoalTradeMarketPrice) * totalOfferTradeMarketPrice;

            AddStockItem(new StockItem
            (
                goal.ItemData,
                goal.ItemQuality,
                goal.ItemRarity,
                goal.Amount,
                newValue,
                goal.Amount * newValue
            ));

        }

        foreach (var offer in offers)
        {

            float marketUnitPrice = offer.MarketUnitPrice;
            float marketTotalPrice = marketUnitPrice * offer.Amount;

            float newValue = (marketTotalPrice / totalOfferTradeMarketPrice) * totalGoalTradeMarketPrice;
            AddStockItem(new StockItem
            (
                offer.ItemData,
                offer.ItemQuality,
                offer.ItemRarity,
                offer.Amount,
                newValue,
                offer.Amount * newValue
            ));
        }
    }

    public void AddTransaction(Merchant merchant, List<TradeStockItem> bought, List<TradeStockItem> sold, int karmaPoints, int reputationPoints)
    {
        StockItemsTransactions.Add(new StockItemsTransaction(
            DateTime.Now,
            merchant.MerchantData,
            sold,
            bought,
            karmaPoints,
            reputationPoints
        ));
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

    public StockItem GetStockItem(StockItem stockItem)
    {
        return StockItems.Where(si => si == stockItem).FirstOrDefault();
    }



}
