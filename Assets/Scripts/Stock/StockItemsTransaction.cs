using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class StockItemsTransaction
{
    //datum
    public DateTime Created;
    //merchantData
    public MerchantData MerchantData;
    //stock item koupen�
    public List<TradeStockItem> StockItemsSold;
    public List<TradeStockItem> StockItemsBought;
    //karma points
    public float KarmaPoints;
    //reputation points
    public float ReputationPoints;

    public StockItemsTransaction(DateTime created, MerchantData merchantData, List<TradeStockItem> stockItemsSold, List<TradeStockItem> stockItemsBought, float karmaPoints, float reputationPoints)
    {
        Created = created;
        MerchantData = merchantData;
        StockItemsSold = stockItemsSold;
        StockItemsBought = stockItemsBought;
        KarmaPoints = karmaPoints;
        ReputationPoints = reputationPoints;
    }


    /*
      P�ijde� pak k Merchantovi a kdy� si zaplat� dostatek trading point�, tak se m��e� pod�vat na jeho posledn� obchody
          - zjist�, �e t�eba prodat 10 chleba za 5 jablek
          - kdy� to budou tvoje obchody (hr��e), tak se samo uk�ou, proto�e ty jsi s n�m provedl ty   
    */
}
