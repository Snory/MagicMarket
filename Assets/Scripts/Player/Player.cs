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

    public void AddTransaction(List<TradeStockItem> sold, List<TradeStockItem> bought)
    {

        float totalSoldPrice = sold.Sum(g => g.MerchantTotalPrice); //this is how merchant valued the stuff he sold
        float totalBoughtPrice = bought.Sum(o => o.MerchantTotalPrice); //this is how merchant valued the stuff he bought

        foreach (var soldItem in sold) //remove from stock excatly what i sold
        {
            float newValue = (soldItem.MerchantTotalPrice / totalSoldPrice) * totalBoughtPrice;

            RemoveStockItem(new StockItem
            {
                ItemData = soldItem.ItemData,
                ItemQuality = soldItem.ItemQuality,
                ItemRarity = soldItem.ItemRarity,
                Amount = soldItem.Amount,
                TotalPrice = soldItem.TotalPrice,
                UnitPrice = soldItem.UnitPrice
            });
        }

        foreach (var boughtItem in bought)
        {
            float newValue = (boughtItem.TotalPrice / totalBoughtPrice) * totalSoldPrice;
            StockItem newItem = new StockItem
            {
                ItemData = boughtItem.ItemData,
                ItemQuality = boughtItem.ItemQuality,
                ItemRarity = boughtItem.ItemRarity,
                Amount = boughtItem.Amount,
                TotalPrice = boughtItem.Amount * newValue,
                UnitPrice = newValue
            };

            AddStockItem(newItem);
        }
    }

    private void AddStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        if (currentStockItem == null)
        {
            StockItems.Add(item);
        }
        else
        {
            currentStockItem.TotalPrice += item.TotalPrice;
            currentStockItem.Amount += item.Amount;
            currentStockItem.UnitPrice = currentStockItem.TotalPrice / currentStockItem.Amount;
        }
    }

    private void RemoveStockItem(StockItem item)
    {
        StockItem currentStockItem = StockItems.Where(i => i == item).FirstOrDefault();

        currentStockItem.TotalPrice -= item.TotalPrice;
        currentStockItem.Amount -= item.Amount;
        currentStockItem.UnitPrice = currentStockItem.TotalPrice / currentStockItem.Amount;

    }

}

