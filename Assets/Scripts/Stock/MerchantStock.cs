using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantStock : MonoBehaviour
{
    private List<StockItem> _stockItems;

    public void OnGameplayLoaded(List<StockItem> stockItems) 
    {
        _stockItems = stockItems;
    }
}
