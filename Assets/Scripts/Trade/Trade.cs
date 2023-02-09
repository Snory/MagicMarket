using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Trade : MonoBehaviour
{
    [SerializeField] private GameData _gameData;
    [SerializeField] private TradeSessionData _sessionData;

    [SerializeField] private List<StockItem> _goalStockItems;
    [SerializeField] private List<StockItem> _offerStockItems;
        
    [Header("UI")]
    [SerializeField] private GeneralEvent GoalSelectionInitiated;
    [SerializeField] private GeneralEvent OfferSelectionInitiated;
  
    private GUIStyle guiStyle = new GUIStyle();

    private float _goalValue = 0;
    private float _offerValue = 0;
    private float _marketKnowledge = 0;
    private float _karmaPoints = 0;
    private int _negotiationPoints;

    [Header("Test variables")]
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
        CalculateMarketKnowledge();
        CalculateNegotiationPoints();
        CalculateKarmaPoints();

        //send event to UI that negotiation score was updated

        //send event to UI that market knowledge score was updated

    }

    private void CalculateKarmaPoints()
    {
        Market market = _gameData.Market;


        float goalValue = 0;

        foreach (var goal in _goalStockItems)
        {
            float marketUnitPrice = market.GetStockItem(goal).UnitPrice;
            goalValue += goal.Amount * marketUnitPrice;
        }

        float offerValue = 0;

        foreach (var offer in _offerStockItems)
        {
            float marketUnitPrice = market.GetStockItem(offer).UnitPrice;
            offerValue += offer.Amount * marketUnitPrice;
        }

        if(offerValue == goalValue)
        {
            _karmaPoints = 0;
        } else if(offerValue > goalValue)
        {
            _karmaPoints = 1;
        } else
        {
            _karmaPoints = -1;
        }

    }

    private void CalculateNegotiationPoints()
    {
        if (_offerValue >= _goalValue)
        {
            _negotiationPoints = 1;
        }
        else
        {
            _negotiationPoints = -1;
        }
    }

    private void CalculateMarketKnowledge()
    {
        Market market = _gameData.Market;

        if(market == null)
        {
            Debug.LogError("Missing market!");
        }

        float goalValue = 0;

        foreach (var goal in _goalStockItems)
        {
            float marketUnitPrice = market.GetStockItem(goal).UnitPrice;
            goalValue += goal.Amount * marketUnitPrice;
        }

        float offerValue = 0;

        foreach (var offer in _offerStockItems)
        {
            float marketUnitPrice = market.GetStockItem(offer).UnitPrice;
            offerValue += offer.Amount * marketUnitPrice;
        }


        _marketKnowledge = (offerValue / goalValue) > 1 ? (goalValue/ offerValue) : (offerValue / goalValue);
    }

    public void OnGUI()
    {
        guiStyle.fontSize = 20;
        GUI.Label(new Rect(10, 10, 200, 40), $"Merchant value: {_merchantExpectedValue}", guiStyle);
        GUI.Label(new Rect(10, 50, 200, 40), $"Player value: {_playerSelectedValue}", guiStyle);
        GUI.Label(new Rect(10, 90, 200, 40), $"Negotiation points: {_negotiationPoints}", guiStyle);
        GUI.Label(new Rect(10, 130, 200, 40), $"Market knowledge: {_marketKnowledge * 100} %", guiStyle);
        GUI.Label(new Rect(10, 170, 200, 40), $"Karma points: {_karmaPoints}", guiStyle);
    }

    public void OnFinishTrade()
    {
        //based on the negotiation points determine if it was sucessful trade or not
        Player player = _sessionData.Player;
        player.ReputationPoints += _negotiationPoints >= 0 ? 1 : -1;


        //update market stock prices
        _gameData.Market.AddTransaction(_goalStockItems, _offerStockItems);




        //update merchant stock

        //update merchant market knowledge

        //update player stock

        //update player reputation points and karma points


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

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(goal);

            if(itemMarketKnowledge == null)
            {
                if (_sessionData.PlayerBuying) //selling merchant items
                {
                    Debug.LogError($"Merchant {merchant.MerchantData.Identification} should not value of his own item {goal.ItemData.Identification}!");
                } else //buying items from player
                {
                    //player is telling me it cost "XX", but merchant market knowledge know the real value!
                    value = Random.Range(goal.UnitPrice - (goal.UnitPrice * (1 - merchant.CurrentGeneralMarketKnowledge)), goal.UnitPrice + (goal.UnitPrice * (1 - merchant.CurrentGeneralMarketKnowledge)));
                }


                //this will store it right after first value proposal
                    //i will leave it here, it could create interesting situations
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

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(offer);

            if (itemMarketKnowledge == null)
            {

                if (_sessionData.PlayerBuying) //selling merchant items
                {
                    //player is telling me it cost "XX", but merchant market knowledge know the real value!
                    value = Random.Range(offer.UnitPrice - (offer.UnitPrice * (1 - merchant.CurrentGeneralMarketKnowledge)), offer.UnitPrice + (offer.UnitPrice * (1 - merchant.CurrentGeneralMarketKnowledge)));
                }
                else //buying items from player
                {
                    Debug.LogError($"Merchant {merchant.MerchantData.Identification} should not value of his own item {offer.ItemData.Identification}!");
                }

                //this will store it right after first value proposal
                    //i will leave it here, it could create interesting situations
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


    public void OnGameDataSent(EventArgs eventArgs)
    {
        GameDataEventArgs gameDataEventArgs = (GameDataEventArgs) eventArgs;

        _gameData = gameDataEventArgs.GameData;
    }




}
