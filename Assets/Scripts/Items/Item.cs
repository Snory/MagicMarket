using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ItemType { CRAFTED, PRIMARY}

[Serializable]
[JsonConverter(typeof(ItemJsonConverter))]
public class Item 

{
    public string Name;
    public string Identification;
    public string Description;
    public ItemType Type;
    public float ProductionTimeSeconds;
    public float ProductionExperience;
    public List<ItemRarityProbability> ItemRarityProbabilities;
    public List<ItemQualityProbability> ItemQualityProbabilities;


}
