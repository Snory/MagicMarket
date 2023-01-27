using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class StockItem
{
    public ItemData ItemData;
    public float Amount;
    public float UnitPrice;
    public float TotalPrice;
    public ItemRarity Rarity;
    public ItemQuality Quality;
}
