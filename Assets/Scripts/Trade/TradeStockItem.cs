using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class TradeStockItem : StockItem
{
    public float MerchantUnitPrice;
    public float MerchantTotalPrice { get => MerchantUnitPrice * Amount; }

    public float PlayerUnitPrice;
    public float PlayerTotalPrice { get => PlayerUnitPrice * Amount; }

    public float MarketUnitPrice;
    public float MarketTotalPrice { get => MerchantUnitPrice * Amount; }

    public TradeStockItem(StockItem item)
    {
        Amount = item.Amount;
        TotalTradePower = item.TotalTradePower;
        UnitTradePower = item.UnitTradePower;
        ItemRarity = item.ItemRarity;
        ItemQuality = item.ItemQuality;
        ItemData = item.ItemData;
    }
}

