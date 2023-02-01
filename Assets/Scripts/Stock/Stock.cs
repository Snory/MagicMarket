using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class Stock : MonoBehaviour
{
    [SerializeField]
    protected GeneralEvent _StockChanged;
    protected GameData _gameData;
    protected List<StockItem> _stockItems;

    public virtual void OnGameDataSent(EventArgs args)
    {
        GameDataEventArgs gameDataEventArgs = args as GameDataEventArgs;
        _stockItems = gameDataEventArgs.GameData.Player.StockItems;
        _gameData = gameDataEventArgs.GameData;
        RaisePlayerStockChanged();
    }

    public virtual void RaisePlayerStockChanged()
    {
        _StockChanged.Raise(new StockChangedEventArgs(_stockItems));
    }
}

