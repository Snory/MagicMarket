

using System;
using UnityEngine;


[Serializable]
public class StockItem
{
    public ItemData ItemData;
    public float Amount;
    public float UnitPrice;
    public float TotalPrice;
    public ItemQuality ItemQuality;
    public ItemRarity ItemRarity;
}