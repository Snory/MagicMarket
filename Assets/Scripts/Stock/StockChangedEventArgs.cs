using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class StockChangedEventArgs : EventArgs
{
    public List<StockItem> StockItems;

    public StockChangedEventArgs(List<StockItem> stockItems)
    {
        StockItems = stockItems;
    }
}

