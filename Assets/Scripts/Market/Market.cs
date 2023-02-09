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
    public void AddTransaction(List<StockItem> goals, List<StockItem> offers)
    {

        float totalGoalValue = goals.Sum(g => g.TotalPrice);
        float totalOfferValue = offers.Sum(o => o.TotalPrice);

        foreach (var goal in goals)
        {
            float newValue = (goal.TotalPrice / totalGoalValue) * totalOfferValue;
            AddStockItem(new StockItem
            {
                ItemData = goal.ItemData,
                ItemQuality = goal.ItemQuality,
                ItemRarity = goal.ItemRarity,
                Amount = goal.Amount,
                TotalPrice = goal.Amount * newValue,
                UnitPrice = newValue
            });
           
        }

        foreach (var offer in offers)
        {
            float newValue = (offer.TotalPrice / totalOfferValue) * totalGoalValue;
            AddStockItem(new StockItem
            {
                ItemData = offer.ItemData,
                ItemQuality = offer.ItemQuality,
                ItemRarity = offer.ItemRarity,
                Amount = offer.Amount,
                TotalPrice = offer.Amount * newValue,
                UnitPrice = newValue
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
            currentStockItem.TotalPrice += item.TotalPrice;
            currentStockItem.Amount += item.Amount;
            currentStockItem.UnitPrice = currentStockItem.TotalPrice / currentStockItem.Amount;
        }
    }

    public StockItem GetStockItem(StockItem stockItem)
    {
        return StockItems.Where(si => si == stockItem).FirstOrDefault();
    }


   
}
