using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradeStockItemButton : MonoBehaviour
{
   [SerializeField] private GeneralEvent _eventToRaise; 

   public virtual void OnButtonClick()
    {
        _eventToRaise.Raise();
    }

}
