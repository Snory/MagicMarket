using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeStockItemButton : MonoBehaviour
{
    [SerializeField] private GeneralEvent _eventToRaise;

    [SerializeField] private StockItem _stockItem;



    public virtual void OnButtonClick()
    {
        _eventToRaise.Raise(new TradeStockItemEventArgs(_stockItem));
    }

    public void SetStockItem(StockItem item)
    {
        _stockItem = item;
    }

}
