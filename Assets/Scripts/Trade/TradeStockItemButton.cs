using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TradeStockItemButton : MonoBehaviour
{
    [SerializeField] private StockItem _stockItem;
    private Action<TradeStockItemButton> _callback;

    [SerializeField] private TextMeshProUGUI _amountSelected;
    [SerializeField] private GameObject _amountPanel;
    [SerializeField] private GeneralEvent _tradeStockItemChanged;

    private void Awake()
    {
        _amountPanel.SetActive(false);
    }

    public virtual void OnButtonClick()
    {
        _callback(this);
    }

    public void SetStockItem(StockItem item, Action<TradeStockItemButton> callback)
    {
        _stockItem = item;
        _callback = callback;
    }

    public StockItem GetStockItem()
    {
        return _stockItem;
    }

    public void SetAmountSelected(int amount)
    {
        _amountSelected.text = amount.ToString();
        _amountPanel.SetActive(true);
    }

    public GeneralEvent GetTradeStockItemChangedEvent()
    {
        return _tradeStockItemChanged;
    }

}
