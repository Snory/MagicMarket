using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class Player : IStockable
{
    public List<StockItem> StockItems;
    public List<StockItemMarketKnowledge> ItemMarketKnowledge;
    public int ReputationPoints;
    public int KarmaPoints;
    public int TradingPoints;
    public int TradePower;

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

    public void UpdateTadePower(int value)
    {
        TradePower += value;
    }

    public void RemoveStockItem(StockItem item)
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

