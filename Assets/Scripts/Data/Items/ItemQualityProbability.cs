using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public enum ItemQuality
{ LOW, MEDIUM, HIGH }

[Serializable]
public class ItemQualityProbability 
{
    public ItemQuality Quality;

    [Range(0,1)]
    public float Probability;
}
