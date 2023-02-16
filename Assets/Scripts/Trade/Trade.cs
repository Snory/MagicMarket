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
    [SerializeField] private GeneralEvent NegotiationPointsChanged;

    private GUIStyle guiStyle = new GUIStyle();

    private float _goalValue = 0;
    private float _offerValue = 0;
    private float _marketScale = 0;
    private int _karmaPoints = 0;
    private float _negotiationScale;
    private int _reputationPoints = 0;

    [Header("Test variables")]
    [SerializeField] private float _marketOfferValue;
    [SerializeField] private float _marketGoalValue;


    private void Awake()
    {
        _offerTradeStockItems = new List<TradeStockItem>();
        _goalTradeStockItems = new List<TradeStockItem>();

    }

    private void Start()
    {
        RaiseTraideInitiated();
        _goalValue = 0;
        _offerValue = 0;

        CalculateMetrics();
    }

    public void OnEvaluate()
    {
        CalculateMetrics();
    }

    public void OnGoalStockItemChanged(EventArgs args)
    {
        StockItemChanged(args, _goalTradeStockItems);
    }

    public void OnOfferStockItemChanged(EventArgs args)
    {
        StockItemChanged(args, _offerTradeStockItems);
    }

    private void CalculateMetrics()
    {

        CalculateGoalValue();
        CalculateOfferValue();

        CalculateMarketScale();
        CalculateNegotiationScale();
        CalculateKarmaPoints();
        CalculateReputationPoints();
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
            _karmaPoints = (int)((offerValue - goalValue)/15);
        }
        else
        {
            _karmaPoints = -1 * (int)((goalValue - offerValue)/15);
        }
    }

    private void CalculateReputationPoints()
    {

         if (_offerValue == _goalValue)
        {
            _reputationPoints = 0;
        }
        else if (_offerValue > _goalValue)
        {
            _reputationPoints = (int)((_offerValue - _goalValue) / 15);
        }
        else
        {
            _reputationPoints = -1 * (int)((_goalValue - _offerValue) / 15);
        }
    }

    private void CalculateNegotiationScale()
    {
        _negotiationScale = Math.Clamp((_offerValue / _goalValue)-1, -1, 1);

        NegotiationPointsChanged.Raise(new NegotiationPointsEventArgs(_negotiationScale));

    }

    private void CalculateMarketScale()
    {

        float goalValue = 0;

        foreach (var goal in _goalTradeStockItems)
        {
            goalValue += goal.MarketTotalPrice;
        }
        _marketGoalValue = goalValue;

        float offerValue = 0;

        foreach (var offer in _offerTradeStockItems)
        {
            offerValue += offer.MarketTotalPrice;
        }
        _marketOfferValue = offerValue;

        _marketScale = Math.Clamp((offerValue / goalValue)-1, -1, 1);
    }

    public void OnGUI()
    {
        guiStyle.fontSize = 20;
        GUI.Label(new Rect(10, 10, 200, 40), $"Goal value: {_goalValue}", guiStyle);
        GUI.Label(new Rect(10, 50, 200, 40), $"Offer value: {_offerValue}", guiStyle);
        GUI.Label(new Rect(10, 90, 200, 40), $"Market goal value: {_marketGoalValue}", guiStyle);
        GUI.Label(new Rect(10, 130, 200, 40), $"Market offer value: {_marketOfferValue}", guiStyle);
        GUI.Label(new Rect(10, 170, 200, 40), $"Negotiation scale: {Math.Ceiling(_negotiationScale * 100)} %", guiStyle);
        GUI.Label(new Rect(10, 210, 200, 40), $"Reputation points: {_reputationPoints}", guiStyle);
        GUI.Label(new Rect(10, 250, 200, 40), $"Market scale: {Math.Ceiling(_marketScale * 100)} %", guiStyle);
        GUI.Label(new Rect(10, 290, 200, 40), $"Karma points: {_karmaPoints}", guiStyle);
    }

    public void OnFinishTrade()
    {
        //based on the negotiation points determine if it was sucessful trade or not
        Player player = _sessionData.Player;

        Merchant merchant = _sessionData.Merchant;

        if(_negotiationScale >= 0)
        {
            //update market stock prices
            _gameData.Market.AddTransaction(_goalTradeStockItems, _offerTradeStockItems);

            //update merchant stock
            merchant.CloseTrade(_goalTradeStockItems, _offerTradeStockItems);

            //update player stock
            player.CloseTrade(_offerTradeStockItems, _goalTradeStockItems);

        }

        //update stats
        player.ReputationPoints += _reputationPoints;
        player.KarmaPoints += _karmaPoints;
       
        //update player reputation points and karma points
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
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
                value = Random.Range(offer.UnitTradePower - (offer.UnitTradePower * (1 - merchant.GeneralMarketKnowledge)), offer.UnitTradePower + (offer.UnitTradePower * (1 - merchant.GeneralMarketKnowledge)));
                offer.PlayerUnitPrice = offer.UnitTradePower;

                //this will store it right after first value proposal
                //i will leave it here, it could create interesting situations
                merchant.UpdateStockItemKnowledge(offer, value);
            }
            else
            {
                value = itemMarketKnowledge.UnitTradePower;
            }

            _offerValue += offer.Amount * value;
            offer.MerchantUnitPrice = value;
        }
    }

    private void RaiseTraideInitiated()
    {
        GoalSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Merchant.StockItems));
        OfferSelectionInitiated.Raise(new TradeStockEventArgs(_sessionData.Player.StockItems));

    }


    public void OnGameDataSent(EventArgs eventArgs)
    {
        GameDataEventArgs gameDataEventArgs = (GameDataEventArgs)eventArgs;

        _gameData = gameDataEventArgs.GameData;
    }
}
