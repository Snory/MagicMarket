using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TradePhaseHandle : MonoBehaviour
{
    [SerializeField] private TradeSessionData _sessionData;

    [SerializeField] private GeneralEvent GoalSelectionInitiated;
    [SerializeField] private GeneralEvent TradeStarted;

    // Start is called before the first frame update
    void Start()
    {
        RaiseGoalSelectionInitiated(); 
    }

    private void RaiseGoalSelectionInitiated()
    {
        if (_sessionData.PlayerBuying)
        {
            GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));
        } else
        {
            GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));
        }
    }

    public void OnGoalConfirmed()
    {
        Debug.Log("Confirmed");
    }

}