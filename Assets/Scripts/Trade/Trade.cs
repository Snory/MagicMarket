using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trade : MonoBehaviour
{
    [SerializeField] private TradeSessionData _sessionData;

    [SerializeField] private List<StockItem> _goalStockItems;
    [SerializeField] private List<StockItem> _offerStockItems;

    [SerializeField] private int _negotiationPoints;


    [Header("Event for UI")]
    [SerializeField] private GeneralEvent GoalSelectionInitiated;
    [SerializeField] private GeneralEvent OfferSelectionInitiated;

    private void Start()
    {
        RaiseGoalSelectionInitiated();
    }

    public void OnGoalStockItemChanged(EventArgs args)
    {
        TradeStockItemEventArgs stockItemArgs = args as TradeStockItemEventArgs;
        StockItem stockItem = stockItemArgs.StockItem;

        if (_goalStockItems.Contains(stockItem))
        {
            _goalStockItems.Remove(stockItem);
        }

        _goalStockItems.Add(stockItem);
    }

    public void OnOfferStockItemChanged(EventArgs args)
    {
        TradeStockItemEventArgs stockItemArgs = args as TradeStockItemEventArgs;
        StockItem stockItem = stockItemArgs.StockItem;

        if (_offerStockItems.Contains(stockItem))
        {
            _offerStockItems.Remove(stockItem);
        }

        _offerStockItems.Add(stockItem);
    }

    public void OnOfferConfirmed()
    {
        float goalValue = 0;
        float offerValue = 0;

        foreach (var goal in _goalStockItems)
        {
            goalValue += goal.TotalPrice;
        }

        foreach (var offer in _offerStockItems)
        {
            offerValue += offer.TotalPrice;
        }


        if(offerValue >= goalValue)
        {
            _negotiationPoints += 1;
        } else
        {
            _negotiationPoints -= 1;
        }

        //send event to UI that negotiation score was updated

        //send event to UI that market knowledge score was updated

    }

    public void OnSucessTrade()
    {
        //send into to stock market so it can take into consideration the exchanged items
        //set the reputation value
    }

    public void OnFailedTrade()
    {
        //lower the reputation value
    }


    public void OnGoalConfirmed()
    {
        if (_sessionData.PlayerBuying)
        {
            OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));
        }
        else
        {
            OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));
        }
    }

    private void RaiseGoalSelectionInitiated()
    {
        Debug.Log("Goal selection raised");

        if (_sessionData.PlayerBuying)
        {
            GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));
        }
        else
        {
            GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));
        }
    }




}
