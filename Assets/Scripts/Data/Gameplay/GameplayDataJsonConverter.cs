using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameplayDataJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(GameplayData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        JObject input = JObject.Load(reader);

        GameplayData result = new GameplayData
        {
            Identification = (string)input["Identification"],
            GameData = new GameData
            {
                Identification = (string)input["GameData"]["Identification"]
            },
            Merchants = ((JArray)input["Merchants"]).Select(m => new Merchant
            {
                MerchantData = new MerchantData
                {
                    Identification = (string)m["MerchantData"]["Identification"]
                },
                MerchantStockItems = m["MerchantStockItems"] != null && m["MerchantStockItems"].HasValues
                    ? ((JArray)input["MerchantStockItems"]).Select(msi => new StockItem
                    {
                        ItemData = new ItemData
                        {
                            Identification = (string)input["Identification"]
                        },
                        Amount = (float)input["Amount"],
                        Quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)input["Quality"]),
                        Rarity = (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)input["Rarity"]),
                        TotalPrice = (float)input["TotalPrice"],
                        UnitPrice = (float)input["UnitPrice"]
                    }).ToList() : new List<StockItem>()
            }).ToList()
        };

        return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        GameplayData input = (GameplayData)value;

        JObject gameplaydata = new JObject();
        JObject gameData = new JObject();
        
        gameplaydata.Add("Identification", input.Identification);
        gameData.Add("Identification", input.GameData.Identification);
        gameplaydata.Add("GameData", gameData);

        JArray merchants = new JArray();
        foreach (var merchant in input.Merchants)
        {
            JObject merchantData = new JObject();
            merchantData.Add("Identification", merchant.MerchantData.Identification);

            JArray merchantStockItems = new JArray();
            foreach (var stockItem in merchant.MerchantStockItems)
            {
                JObject itemData = new JObject();
                itemData.Add("Identification", stockItem.ItemData.Identification);
                itemData.Add("Amount", stockItem.Amount);
                itemData.Add("Quality", stockItem.Quality.ToString());
                itemData.Add("Rarity", stockItem.Rarity.ToString());
                itemData.Add("TotalPrice", stockItem.TotalPrice);
                itemData.Add("UnitPrice", stockItem.UnitPrice);

                merchantStockItems.Add(itemData);
            }

            JObject merchantObject = new JObject();
            merchantObject.Add("MerchantData", merchantData);
            merchantObject.Add("MerchantStockItems", merchantStockItems);

            merchants.Add(merchantObject);
        }

        gameplaydata.Add("Merchants", merchants);

        gameplaydata.WriteTo(writer);
    }
}
