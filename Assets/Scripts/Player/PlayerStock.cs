using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStock : MonoBehaviour
{
    [SerializeField]
    private GeneralEvent _playerStockChanged;
    private GameData _gameData;
    private List<StockItem> _playerStockItems;

    public void OnGameDataSent(EventArgs args)
    {
        GameDataEventArgs gameDataEventArgs = args as GameDataEventArgs;
        _playerStockItems = gameDataEventArgs.GameData.Player.StockItems;
        _gameData = gameDataEventArgs.GameData;
        RaisePlayerStockChanged();
    }

    public void RaisePlayerStockChanged()
    {
        _playerStockChanged.Raise(new StockChangedEventArgs(_playerStockItems));
    }

}
