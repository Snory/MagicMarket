using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[SerializeField]
public enum ItemType { CRAFTED, PRIMARY}

[Serializable]
[JsonConverter(typeof(ItemDataJsonConverter))]
public class ItemData : Identity
{
    public string Name;
    public string Description;
    public ItemType Type;
    public List<ItemRarityProbability> ItemRarityProbabilities;
    public List<ItemQualityProbability> ItemQualityProbabilities;
    public string SpriteName;

    public ItemRarity GerRarity()
    {

        ItemRarity itemRarity = ItemRarity.LOW;
        float rarityProbability = Random.Range(0,1);

        float sumProb = 0;
        foreach (var prob in ItemRarityProbabilities.OrderBy(p => p.Probability))
        {
            sumProb += prob.Probability;

            if(sumProb < rarityProbability)
            {
                itemRarity = prob.Rarity;
                break;
            }
        }
        return itemRarity;
    }

    public ItemQuality GetQuality()
    {

        ItemQuality itemQuality = ItemQuality.LOW;
        float rarityProbability = Random.Range(0, 1);

        float sumProb = 0;
        foreach (var prob in ItemQualityProbabilities.OrderBy(p => p.Probability))
        {
            sumProb += prob.Probability;

            if (sumProb < rarityProbability)
            {
                itemQuality = prob.Quality;
                break;
            }
        }
        return itemQuality;
    }


}

