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
    //stock item koupený
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
      Pøijdeš pak k Merchantovi a když si zaplatíš dostatek trading pointù, tak se mùžeš podívat na jeho poslední obchody
          - zjistíš, že tøeba prodat 10 chleba za 5 jablek
          - když to budou tvoje obchody (hráèe), tak se samo ukážou, protože ty jsi s ním provedl ty   
    */
}
