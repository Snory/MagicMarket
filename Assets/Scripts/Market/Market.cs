using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]

public class Market 
{
    public List<StockItem> StockItems;

    public void AddTransaction(List<StockItem> goals, List<StockItem> offers)
    {
        //now i need to calculate the value of bought and sold item
    }

    public StockItem GetStockItem(StockItem stockItem)
    {
        return StockItems.Where(si => si == stockItem).FirstOrDefault();
    }


   
}
