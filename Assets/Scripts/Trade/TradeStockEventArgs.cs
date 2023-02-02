using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TradeStockEventArgs : EventArgs
{
    public List<StockItem> StockItems;

    public TradeStockEventArgs(List<StockItem> stockItems)
    {
        StockItems = stockItems;
    }
}
