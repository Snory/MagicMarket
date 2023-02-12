using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class MerchantData : Identity
{

    public string Name;
    public string Description;
    public string SpriteName;
    public float GeneralMarketKnowledge;
    public List<ItemData> ItemsInterest;



}
