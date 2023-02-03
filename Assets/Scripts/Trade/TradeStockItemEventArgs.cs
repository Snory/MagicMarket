using System;

internal class TradeStockItemEventArgs : EventArgs
{
    public StockItem StockItem;

    public TradeStockItemEventArgs(StockItem stockItem)
    {
        this.StockItem = stockItem;
    }
}