using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class Merchant 
{
    public MerchantData MerchantData;
    public List<StockItem> MerchantStockItems;

    public Merchant()
    {

    }

    public Merchant(MerchantData merchantData, List<StockItem> merchantStockItems)
    {
        MerchantData = merchantData;
        MerchantStockItems = merchantStockItems;
    }

}

