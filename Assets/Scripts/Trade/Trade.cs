using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour
{
    [SerializeField] private List<StockItem> _itemsToBuy;

    public void OnTradeStockItemChanged(EventArgs args)
    {
        TradeStockItemEventArgs stockItemArgs = args as TradeStockItemEventArgs;
        StockItem stockItem = stockItemArgs.StockItem;

        if (_itemsToBuy.Contains(stockItem))
        {
            _itemsToBuy.Remove(stockItem);
        }

        _itemsToBuy.Add(stockItem);
    }
}
