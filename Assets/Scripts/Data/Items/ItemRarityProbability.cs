using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public enum ItemRarity { LOW, MEDIUM, HIGH }

[Serializable]
public class ItemRarityProbability
{
    public ItemRarity Rarity;
    [Range(0,1)]
    public float Probability;
}

