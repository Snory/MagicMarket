using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Trade : MonoBehaviour
{
    [SerializeField] private TradeSessionData _sessionData;

    [SerializeField] private List<StockItem> _goalStockItems;
    [SerializeField] private List<StockItem> _offerStockItems;
        
    [Header("UI")]
    [SerializeField] private GeneralEvent GoalSelectionInitiated;
    [SerializeField] private GeneralEvent OfferSelectionInitiated;
  
    private GUIStyle guiStyle = new GUIStyle();

    private float _goalValue = 0;
    private float _offerValue = 0;


    [Header("Test variables")]
    [SerializeField] private int _negotiationPoints;
    [SerializeField] private float _merchantExpectedValue;
    [SerializeField] private float _playerSelectedValue;


    private void Start()
    {
        RaiseGoalSelectionInitiated();
        _negotiationPoints = -1;
        _goalValue = 0;
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
        CalculateOfferValue();

        //know how to calculate negotiation points   
        if(_offerValue >= _goalValue)
        {
            _negotiationPoints = 1;
        } else
        {
            _negotiationPoints = -1;
        }

        //what if i would compare the offer to goal value and calculate something like player "karma" for 
            //selling something for much lower value than the market value

        

        //send event to UI that negotiation score was updated

        //send event to UI that market knowledge score was updated

    }

    public void OnGUI()
    {
        guiStyle.fontSize = 20;
        GUI.Label(new Rect(10, 10, 200, 40), $"Merchant value: {_merchantExpectedValue}", guiStyle);
        GUI.Label(new Rect(10, 50, 200, 40), $"Player value: {_playerSelectedValue}", guiStyle);
        GUI.Label(new Rect(10, 90, 200, 40), $"Negotiation points: {_negotiationPoints}", guiStyle);
    }

    public void OnFinishTrade()
    {
        //based on the negotiation points determine if it was sucessful trade or not
        Player player = _sessionData.Player;
        player.ReputationPoints += _negotiationPoints >= 0 ? 1 : -1;

        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }


    public void OnGoalConfirmed()
    {
        CalculateGoalValue();

        if (_sessionData.PlayerBuying)
        {
            OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));
        }
        else
        {
            OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));
        }
    }

    private void CalculateGoalValue()
    {

        Merchant merchant = _sessionData.Merchant;
        _goalValue = 0;

        foreach (var goal in _goalStockItems)
        {
            float value = 0;

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(goal.ItemData.Identification);

            if(itemMarketKnowledge == null)
            {
                //apply current market knowledge
                value = Random.Range(value - (value * (1 - merchant.CurrentGeneralMarketKnowledge)), value + (value * (1 - merchant.CurrentGeneralMarketKnowledge)));
                
                //store it as current merchant market knowledge
                merchant.ItemMarketKnowledge.Add(new StockItemMarketKnowledge { ItemData = goal.ItemData, UnitPrice = value });

            } else
            {
                value = itemMarketKnowledge.UnitPrice;
            }


            _goalValue += goal.Amount * value;
        }

        _merchantExpectedValue = _goalValue;
    }

    private void CalculateOfferValue()
    {
        Merchant merchant = _sessionData.Merchant;
        _offerValue = 0;

        foreach (var offer in _offerStockItems)
        {
            float value = 0;

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(offer.ItemData.Identification);

            if (itemMarketKnowledge == null)
            {

                //apply current market knowledge
                    //player is telling me it cost "XX", but merchant market knowledge know the real value!
                value = Random.Range(offer.TotalPrice - (offer.TotalPrice * (1 - merchant.CurrentGeneralMarketKnowledge)), offer.TotalPrice + (offer.TotalPrice * (1 - merchant.CurrentGeneralMarketKnowledge)));

                //store it as current merchant market knowledge
                merchant.ItemMarketKnowledge.Add(new StockItemMarketKnowledge { ItemData = offer.ItemData, UnitPrice = value });

            }
            else
            {
                value = itemMarketKnowledge.UnitPrice;
            }


            _offerValue += offer.Amount * value;
        }

        _playerSelectedValue = _offerValue;
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
