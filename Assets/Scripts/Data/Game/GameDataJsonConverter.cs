using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameDataJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(GameData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject input = JObject.Load(reader);

        GameData result = new GameData
        {
            Identification = (string)input["Identification"],
            Name = (string)input["Name"],
            Merchants = input["Merchants"] != null && input["Merchants"].HasValues
                    ? ((JArray)input["Merchants"]).Select(m => new Merchant
                    {
                        MerchantData = new MerchantData
                        {
                            Identification = (string)m["MerchantData"]["Identification"]
                        },
                        GeneralMarketKnowledge = (float)m["GeneralMarketKnowledge"],
                        StockItems = m["MerchantStockItems"] != null && m["MerchantStockItems"].HasValues
                            ? ((JArray)m["MerchantStockItems"]).Select(s => new StockItem
                            (
                                new ItemData
                                {
                                    Identification = (string)s["ItemData"]["Identification"]
                                },
                                (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                                (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                                (float)s["Amount"],
                                (float)s["UnitTradePower"],
                                (float)s["TotalTradePower"]
                            )).ToList() : new List<StockItem>(),
                        ItemMarketKnowledge = m["MarketItemMarketKnowledge"] != null && m["MarketItemMarketKnowledge"].HasValues
                            ? ((JArray)m["MarketItemMarketKnowledge"]).Select(s => new StockItemMarketKnowledge
                            (
                                new ItemData
                                {
                                    Identification = (string)s["ItemData"]["Identification"]
                                },
                                (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                                (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                                (float)s["UnitTradePower"]
                            )).ToList() : new List<StockItemMarketKnowledge>()

                    }).ToList() : new List<Merchant>(),
            Player = new Player
            {
                StockItems = input["Player"]["PlayerStockItems"] != null && input["Player"]["PlayerStockItems"].HasValues
                    ? ((JArray)input["Player"]["PlayerStockItems"]).Select(s => new StockItem
                    (
                        new ItemData
                        {
                            Identification = (string)s["ItemData"]["Identification"]
                        },
                        (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                        (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                        (float)s["Amount"],
                        (float)s["UnitTradePower"],
                        (float)s["TotalTradePower"]
                    )).ToList() : new List<StockItem>(),
                TradePower = (int)input["Player"]["TradePower"],
                ReputationPoints = (int)input["Player"]["ReputationPoints"],
                KarmaPoints = (int)input["Player"]["KarmaPoints"],
                TradingPoints = (int)input["Player"]["TradingPoints"]
            },
            Market = new Market
            {
                StockItems = input["Market"]["MarketStockItems"] != null && input["Market"]["MarketStockItems"].HasValues
                    ? ((JArray)input["Market"]["MarketStockItems"]).Select(s => new StockItem
                    (
                        new ItemData
                        {
                            Identification = (string)s["ItemData"]["Identification"]
                        },
                        (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                        (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                        (float)s["Amount"],
                        (float)s["UnitTradePower"],
                        (float)s["TotalTradePower"]
                    )).ToList() : new List<StockItem>(),
                StockItemsTransactions = input["Market"]["StockItemsTransactions"] != null && input["Market"]["StockItemsTransactions"].HasValues
                    ? ((JArray)input["Market"]["StockItemsTransactions"]).Select(sit => new StockItemsTransaction(
                        (DateTime)sit["Created"],
                        new MerchantData
                        {
                            Identification = (string)sit["MerchantData"]["Identification"]
                        },
                        sit["StockItemsSold"] != null && sit["StockItemsSold"].HasValues ?
                            ((JArray)sit["StockItemsSold"]).Select(s => new StockItem(
                                 new ItemData
                                 {
                                     Identification = (string)s["ItemData"]["Identification"]
                                 },
                                (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                                (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                                (float)s["Amount"],
                                (float)s["UnitTradePower"],
                                (float)s["TotalTradePower"]
                            )).ToList() : new List<StockItem>(),
                    sit["StockItemsSold"] != null && sit["StockItemsSold"].HasValues ?
                            ((JArray)sit["StockItemsSold"]).Select(s => new StockItem(
                                 new ItemData
                                 {
                                     Identification = (string)s["ItemData"]["Identification"]
                                 },
                                (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                                (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"]),
                                (float)s["Amount"],
                                (float)s["UnitTradePower"],
                                (float)s["TotalTradePower"]
                            )).ToList() : new List<StockItem>(),
                            (float) sit["KarmaPoints"],
                            (float) sit["ReputationPoints"]
                      )).ToList() : new List<StockItemsTransaction>()
            }
        };

        return result;

    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        GameData input = (GameData)value;

        JObject gameData = new JObject();
        gameData.Add("Identification", input.Identification);
        gameData.Add("Name", input.Name);

        JArray merchants = new JArray();
        foreach (var merchant in input.Merchants)
        {
            JObject merchantData = new JObject();
            merchantData.Add("Identification", merchant.MerchantData.Identification);

            JArray merchantStockitems = new JArray();

            foreach (var stockItem in merchant.StockItems)
            {
                JObject itemData = new JObject();
                itemData.Add("Identification", stockItem.ItemData.Identification);

                JObject stockItemObject = new JObject();
                stockItemObject.Add("ItemData", itemData);

                stockItemObject.Add("Amount", stockItem.Amount);
                stockItemObject.Add("UnitTradePower", stockItem.UnitTradePower);
                stockItemObject.Add("TotalTradePower", stockItem.TotalTradePower);
                stockItemObject.Add("Quality", stockItem.ItemQuality.ToString());
                stockItemObject.Add("Rarity", stockItem.ItemRarity.ToString());
                merchantStockitems.Add(stockItemObject);
            }

            JArray merchantMarketItemKnowledge = new JArray();

            foreach (var itemMarketKnowledge in merchant.ItemMarketKnowledge)
            {
                JObject itemData = new JObject();
                itemData.Add("Identification", itemMarketKnowledge.ItemData.Identification);

                JObject itemmarketKnowledgeObject = new JObject();
                itemmarketKnowledgeObject.Add("ItemData", itemData);
                itemmarketKnowledgeObject.Add("UnitTradePower", itemMarketKnowledge.UnitTradePower);
                itemmarketKnowledgeObject.Add("Quality", itemMarketKnowledge.ItemQuality.ToString());
                itemmarketKnowledgeObject.Add("Rarity", itemMarketKnowledge.ItemRarity.ToString());
                merchantMarketItemKnowledge.Add(itemmarketKnowledgeObject);
            }


            JObject merchantObject = new JObject();
            merchantObject.Add("MerchantData", merchantData);
            merchantObject.Add("MerchantStockItems", merchantStockitems);
            merchantObject.Add("GeneralMarketKnowledge", merchant.GeneralMarketKnowledge);
            merchantObject.Add("MarketItemMarketKnowledge", merchantMarketItemKnowledge);
            merchants.Add(merchantObject);
        }

        JObject player = new JObject();
        JArray playerStockItems = new JArray();

        foreach (var stockItem in input.Player.StockItems)
        {
            JObject itemData = new JObject();
            itemData.Add("Identification", stockItem.ItemData.Identification);

            JObject stockItemObject = new JObject();
            stockItemObject.Add("ItemData", itemData);

            stockItemObject.Add("Amount", stockItem.Amount);
            stockItemObject.Add("UnitTradePower", stockItem.UnitTradePower);
            stockItemObject.Add("TotalTradePower", stockItem.TotalTradePower);
            stockItemObject.Add("Quality", stockItem.ItemQuality.ToString());
            stockItemObject.Add("Rarity", stockItem.ItemRarity.ToString());
            playerStockItems.Add(stockItemObject);
        }

        player.Add("TradePower", input.Player.TradePower);
        player.Add("TradingPoints", input.Player.TradingPoints);
        player.Add("ReputationPoints", input.Player.ReputationPoints);
        player.Add("KarmaPoints", input.Player.KarmaPoints);

        player.Add("PlayerStockItems", playerStockItems);

        JObject market = new JObject();
        JArray marketStockItems = new JArray();

        foreach (var stockItem in input.Market.StockItems)
        {
            JObject itemData = new JObject();
            itemData.Add("Identification", stockItem.ItemData.Identification);

            JObject stockItemObject = new JObject();
            stockItemObject.Add("ItemData", itemData);

            stockItemObject.Add("Amount", stockItem.Amount);
            stockItemObject.Add("UnitTradePower", stockItem.UnitTradePower);
            stockItemObject.Add("TotalTradePower", stockItem.TotalTradePower);
            stockItemObject.Add("Quality", stockItem.ItemQuality.ToString());
            stockItemObject.Add("Rarity", stockItem.ItemRarity.ToString());
            marketStockItems.Add(stockItemObject);
        }

        market.Add("MarketStockItems", marketStockItems);

        JArray marketStockItemTransactions = new JArray();

        foreach (var stockItemTransaction in input.Market.StockItemsTransactions)
        {
            JObject stockItemTransactionObject = new JObject();
            stockItemTransactionObject.Add("Created", stockItemTransaction.Created);

            JObject merchantData = new JObject();
            merchantData.Add("Identification", stockItemTransaction.MerchantData.Identification);
            stockItemTransactionObject.Add("MerchantData", merchantData);

            JArray stockItemsSold = new JArray();

            foreach (var stockItemSold in stockItemTransaction.StockItemsSold)
            {
                JObject stockitemSoldObject = new JObject();
                stockitemSoldObject.Add("Amount", stockItemSold.Amount);
                stockitemSoldObject.Add("UnitTradePower", stockItemSold.UnitTradePower);
                stockitemSoldObject.Add("TotalTradePower", stockItemSold.TotalTradePower);
                stockitemSoldObject.Add("Quality", stockItemSold.ItemQuality.ToString());
                stockitemSoldObject.Add("Rarity", stockItemSold.ItemRarity.ToString());

                stockItemsSold.Add(stockitemSoldObject);

            }

            JArray stockItemsBought = new JArray();

            foreach (var stockItemBought in stockItemTransaction.StockItemsBought)
            {
                JObject stockitemBoughtObject = new JObject();
                stockitemBoughtObject.Add("Amount", stockItemBought.Amount);
                stockitemBoughtObject.Add("UnitTradePower", stockItemBought.UnitTradePower);
                stockitemBoughtObject.Add("TotalTradePower", stockItemBought.TotalTradePower);
                stockitemBoughtObject.Add("Quality", stockItemBought.ItemQuality.ToString());
                stockitemBoughtObject.Add("Rarity", stockItemBought.ItemRarity.ToString());

                stockItemsBought.Add(stockitemBoughtObject);
            }

            stockItemTransactionObject.Add("StockItemsSold", stockItemsSold);
            stockItemTransactionObject.Add("StockItemsBought", stockItemsBought);

            stockItemTransactionObject.Add("KarmaPoints", stockItemTransaction.KarmaPoints);
            stockItemTransactionObject.Add("ReputationPoints", stockItemTransaction.ReputationPoints);


            marketStockItemTransactions.Add(stockItemTransactionObject);

        }

        market.Add("StockItemsTransactions", marketStockItemTransactions);

        gameData.Add("Merchants", merchants);
        gameData.Add("Player", player);
        gameData.Add("Market", market);
        gameData.WriteTo(writer);
    }
}
