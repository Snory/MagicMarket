using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeStockItemSelectionUI : MonoBehaviour
{
    [SerializeField] private TradeUI _mainUI;


    [Header("Stock item selection")]
    [SerializeField] private GameObject _stockPanel;
    [SerializeField] private CanvasGroup _panelGroup;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _stockItem;
    [SerializeField] private GeneralEvent _confirmedEvent;

    [Header("Amount selection")]
    [SerializeField] private GameObject _amountSelectionPanel;
    [SerializeField] private Slider _amountSelectionSlider;
    [SerializeField] private GeneralEvent _tradeItemStockChanged;
    [SerializeField] private TradeStockItemButton _tradeStockItemButton;
    [SerializeField] private TextMeshProUGUI _amount;

    public void OnSelectionInitiated(EventArgs args)
    {

        TradeStockEventArgs argsStock = (TradeStockEventArgs)args;
        _stockPanel.SetActive(true);

        Transform[] children = _content.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.parent == _content.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (var stockItem in argsStock.StockItems)
        {
            GameObject contentItem = Instantiate(_stockItem, _content.transform);
            contentItem.GetComponent<TradeStockItemButton>().SetStockItem(stockItem, OnAmountSelection);

            Sprite sprite = _mainUI.GetSprite(stockItem.ItemData.SpriteName);

            if (sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }

        _panelGroup.interactable = true;
    }


    public void OnSliderAmountChanged()
    {
        _amount.text = _amountSelectionSlider.value.ToString();
    }

    public void OnAmountSelection(TradeStockItemButton stockItemButton)
    {
        _panelGroup.interactable = false;
        _amountSelectionPanel.SetActive(true);
        _tradeStockItemButton = stockItemButton;
        _amountSelectionSlider.minValue = 1;
        _amountSelectionSlider.maxValue = _tradeStockItemButton.GetStockItem().Amount;
    }

    public void OnAmountConfirmed()
    {
        StockItem selectedStockItem = _tradeStockItemButton.GetStockItem();

        StockItem stockItem = new StockItem
        (
            selectedStockItem.ItemData,
            selectedStockItem.ItemQuality,
            selectedStockItem.ItemRarity,
            _amountSelectionSlider.value,
            selectedStockItem.UnitTradePower,
            _amountSelectionSlider.value * selectedStockItem.UnitTradePower
        );

        _tradeItemStockChanged.Raise(new TradeStockItemEventArgs(stockItem));
        _tradeStockItemButton.SetAmountSelected((int)_amountSelectionSlider.value);
        _amountSelectionPanel.SetActive(false);
        _panelGroup.interactable = true;
    }


    public void OnAmountBack()
    {
        _panelGroup.interactable = true;
        _amountSelectionPanel.SetActive(false);
    }
}
