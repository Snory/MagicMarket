using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField]
public enum ItemType { CRAFTED, PRIMARY}

[Serializable]
[JsonConverter(typeof(ItemDataJsonConverter))]
public class ItemData : Identity
{
    public string Name;
    public string Description;
    public ItemType Type;
    public float ProductionTimeSeconds;
    public float ProductionExperience;
    public List<ItemRarityProbability> ItemRarityProbabilities;
    public List<ItemQualityProbability> ItemQualityProbabilities;

}

