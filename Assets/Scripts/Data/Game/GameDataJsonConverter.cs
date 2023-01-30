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
                StockItems = m["MerchantStockItems"] != null && m["MerchantStockItems"].HasValues
                    ? ((JArray)m["MerchantStockItems"]).Select(s => new StockItem
                {
                    ItemData = new ItemData
                    {
                        Identification = (string) s["ItemData"]["Identification"]
                    },
                    Amount = (float)s["Amount"],
                    UnitPrice = (float)s["UnitPrice"],
                    TotalPrice = (float)s["TotalPrice"],
                    ItemQuality = (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                    ItemRarity = (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"])
                }).ToList() : new List<StockItem>()

            }).ToList() : new List<Merchant>(),
            Player = new Player
            {
                StockItems = input["Player"]["PlayerStockItems"] != null && input["Player"]["PlayerStockItems"].HasValues
                    ? ((JArray)input["Player"]["PlayerStockItems"]).Select(s => new StockItem
                    {
                        ItemData = new ItemData
                        {
                            Identification = (string)s["ItemData"]["Identification"]
                        },
                        Amount = (float)s["Amount"],
                        UnitPrice = (float)s["UnitPrice"],
                        TotalPrice = (float)s["TotalPrice"],
                        ItemQuality = (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)s["Quality"]),
                        ItemRarity = (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)s["Rarity"])
                    }).ToList() : new List<StockItem>()
            }
        };

        return result;

    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        GameData input = (GameData) value;

        JObject gameData = new JObject();
        gameData.Add("Identification", input.Identification);
        gameData.Add("Name", input.Name);

        JArray merchants = new JArray();
        foreach(var merchant in input.Merchants)
        {
            JObject merchantData = new JObject();
            merchantData.Add("Identification", merchant.MerchantData.Identification);

            JArray merchantStockitems = new JArray();

            foreach(var stockItem in merchant.StockItems)
            {
                JObject itemData = new JObject();
                itemData.Add("Identification", stockItem.ItemData.Identification);

                JObject stockItemObject = new JObject();
                stockItemObject.Add("ItemData", itemData);

                stockItemObject.Add("Amount", stockItem.Amount);
                stockItemObject.Add("UnitPrice", stockItem.UnitPrice);
                stockItemObject.Add("TotalPrice", stockItem.TotalPrice);
                stockItemObject.Add("Quality", stockItem.ItemQuality.ToString());
                stockItemObject.Add("Rarity", stockItem.ItemRarity.ToString());
                merchantStockitems.Add(stockItemObject);
            }

            JObject merchantObject = new JObject();
            merchantObject.Add("MerchantData", merchantData);
            merchantObject.Add("MerchantStockItems", merchantStockitems);
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
            stockItemObject.Add("UnitPrice", stockItem.UnitPrice);
            stockItemObject.Add("TotalPrice", stockItem.TotalPrice);
            stockItemObject.Add("Quality", stockItem.ItemQuality.ToString());
            stockItemObject.Add("Rarity", stockItem.ItemRarity.ToString());
            playerStockItems.Add(stockItemObject);
        }

        player.Add("PlayerStockItems",playerStockItems);

        gameData.Add("Merchants", merchants);
        gameData.Add("Player", player);
        gameData.WriteTo(writer);
    }
}
