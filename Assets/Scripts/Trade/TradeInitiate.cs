using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TradeInitiate : MonoBehaviour
{
    [SerializeField] private Merchant _selectedMerchant;
    [SerializeField] private Player _player;
    [SerializeField] private TradeSessionData _tradeSessionData;

    /*
     * Should should PlayerStock as he will be offering items to sell in order to buy from merchant
    */
    public void OnBuyButton()
    {
        _tradeSessionData.Player = _player;
        _tradeSessionData.Merchant = _selectedMerchant;
        _tradeSessionData.PlayerBuying = true;
        InitiateTrade();
    }

    /*
     * Should should MerchantStock as he will be offering items to sell in order to buy from player
    */
    public void OnSellButton()
    {
        _tradeSessionData.Player = _player;
        _tradeSessionData.Merchant = _selectedMerchant;
        _tradeSessionData.PlayerBuying = false;
        InitiateTrade();
    }

    public void InitiateTrade()
    {
        SceneManager.LoadScene("Trade", LoadSceneMode.Single);
    }

    public void OnMerchantSelected(EventArgs args)
    {
        MerchantEventArgs merchant = args as MerchantEventArgs;
        _selectedMerchant = merchant.Merchant; 
    }

    public void OnGameDataSent(EventArgs args)
    {
        GameDataEventArgs gamedata = args as GameDataEventArgs;
        _player = gamedata.GameData.Player;
    }

}
