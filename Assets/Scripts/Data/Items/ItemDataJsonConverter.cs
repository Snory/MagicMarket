using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ItemDataJsonConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(ItemData);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // deserialize the JSON into an ItemData object
        JObject item = JObject.Load(reader);
        ItemData result = new ItemData
        {
            Identification = (string)item["Identification"],
            Name = (string)item["Name"],
            Description = (string)item["Description"],
            ProductionTimeSeconds = (float)item["ProductionTimeSeconds"],
            ProductionExperience = (float)item["ProductionExperience"],
            Type = (ItemType)Enum.Parse(typeof(ItemType), (string)item["Type"]),
            ItemRarityProbabilities = ((JArray)item["ItemRarityProbabilities"]).Select(p => new ItemRarityProbability
            {
                Rarity = (ItemRarity)Enum.Parse(typeof(ItemRarity), (string)p["Rarity"]),
                Probability = (float)p["Probability"]
            }).ToList(),
            ItemQualityProbabilities = ((JArray)item["ItemQualityProbabilities"]).Select(p => new ItemQualityProbability
            {
                Quality = (ItemQuality)Enum.Parse(typeof(ItemQuality), (string)p["Quality"]),
                Probability = (float)p["Probability"]
            }).ToList()
        };

        return result;
    }

    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        // serialize the ItemData object into JSON
        ItemData item = (ItemData)value;
        JObject itemJson = new JObject
        {
            {"Identification", item.Identification},
            {"Name", item.Name},
            {"Description", item.Description},
            {"ProductionTimeSeconds", item.ProductionTimeSeconds},
            {"ProductionExperience", item.ProductionExperience},
            {"Type", item.Type.ToString()},
            {"ItemRarityProbabilities", new JArray(item.ItemRarityProbabilities.Select(p => new JObject {{"Rarity", p.Rarity.ToString()}, {"Probability", p.Probability}}))},
            {"ItemQualityProbabilities", new JArray(item.ItemRarityProbabilities.Select(p => new JObject {{"Quality", p.Rarity.ToString()}, {"Probability", p.Probability}}))}
        };

        itemJson.WriteTo(writer);
    }
}