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

    [SerializeField] private List<TradeStockItem> _goalTradeStockItems;
    [SerializeField] private List<TradeStockItem> _offerTradeStockItems;

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


    private void Awake()
    {
        _offerTradeStockItems = new List<TradeStockItem>();
        _goalTradeStockItems = new List<TradeStockItem>();

    }

    private void Start()
    {
        RaiseGoalSelectionInitiated();
        _negotiationPoints = -1;
        _goalValue = 0;
    }

    public void OnGoalStockItemChanged(EventArgs args)
    {
        StockItemChanged(args, _goalTradeStockItems);
    }

    public void OnOfferStockItemChanged(EventArgs args)
    {
        StockItemChanged(args, _offerTradeStockItems);
    }

    private void StockItemChanged(EventArgs args, List<TradeStockItem> stockItems)
    {
        TradeStockItemEventArgs stockItemArgs = args as TradeStockItemEventArgs;
        TradeStockItem stockItem = new TradeStockItem(stockItemArgs.StockItem);
        float marketUnitPrice = _gameData.Market.GetStockItem(stockItem).UnitTradePower;
        stockItem.MarketUnitPrice = marketUnitPrice;


        if (stockItems.Contains(stockItem))
        {
            stockItems.Remove(stockItem);
        }

        stockItems.Add(stockItem);
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

        float goalValue = 0;

        foreach (var goal in _goalTradeStockItems)
        {
            goalValue += goal.Amount * goal.MarketUnitPrice;
        }

        float offerValue = 0;

        foreach (var offer in _offerTradeStockItems)
        {
            offerValue += offer.Amount * offer.MarketUnitPrice;
        }

        if (offerValue == goalValue)
        {
            _karmaPoints = 0;
        }
        else if (offerValue > goalValue)
        {
            _karmaPoints = 1;
        }
        else
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

        float goalValue = 0;

        foreach (var goal in _goalTradeStockItems)
        {
            goalValue += goal.MarketTotalPrice;
        }

        float offerValue = 0;

        foreach (var offer in _offerTradeStockItems)
        {
            offerValue += offer.MarketTotalPrice;
        }

        _marketKnowledge = (offerValue / goalValue) > 1 ? (goalValue / offerValue) : (offerValue / goalValue);
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
        _gameData.Market.AddTransaction(_goalTradeStockItems, _offerTradeStockItems);

        //update merchant stock
        _sessionData.Merchant.AddTransaction(_goalTradeStockItems, _offerTradeStockItems);

        //update player stock
        _sessionData.Player.AddTransaction(_offerTradeStockItems, _goalTradeStockItems);

        //update player reputation points and karma points


        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }


    public void OnGoalConfirmed()
    {
        CalculateGoalValue();
        OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));

    }

    private void CalculateGoalValue()
    {

        Merchant merchant = _sessionData.Merchant;
        _goalValue = 0;

        foreach (var goal in _goalTradeStockItems)
        {

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(goal);
            float value = itemMarketKnowledge.UnitTradePower;
            _goalValue += goal.Amount * value;
            goal.MerchantUnitPrice = value;
        }

        _merchantExpectedValue = _goalValue;
    }

    private void CalculateOfferValue()
    {
        Merchant merchant = _sessionData.Merchant;
        _offerValue = 0;

        foreach (var offer in _offerTradeStockItems)
        {
            float value;

            StockItemMarketKnowledge itemMarketKnowledge = merchant.GetItemMarketKnowledge(offer);

            if (itemMarketKnowledge == null)
            {


                //player is telling me it cost "XX", but merchant market knowledge know the real value!
                value = Random.Range(offer.UnitTradePower - (offer.UnitTradePower * (1 - merchant.CurrentGeneralMarketKnowledge)), offer.UnitTradePower + (offer.UnitTradePower * (1 - merchant.CurrentGeneralMarketKnowledge)));
                offer.PlayerUnitPrice = offer.UnitTradePower;


                //this will store it right after first value proposal
                //i will leave it here, it could create interesting situations
                merchant.ItemMarketKnowledge.Add(new StockItemMarketKnowledge ( offer.ItemData, offer.ItemQuality, offer.ItemRarity, value));

            }
            else
            {
                value = itemMarketKnowledge.UnitTradePower;
            }

            _offerValue += offer.Amount * value;
            offer.MerchantUnitPrice = value;
        }

        _playerSelectedValue = _offerValue;
    }

    private void RaiseGoalSelectionInitiated()
    {
        Debug.Log("Goal selection raised");
        GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));

    }


    public void OnGameDataSent(EventArgs eventArgs)
    {
        GameDataEventArgs gameDataEventArgs = (GameDataEventArgs)eventArgs;

        _gameData = gameDataEventArgs.GameData;
    }
}
