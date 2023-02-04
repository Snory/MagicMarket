using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{

    [Header("Goals")]
    [SerializeField] private GameObject _goalPanel;
    private CanvasGroup _goalPanelGroup;
    [SerializeField] private GameObject _goalContent;
    [SerializeField] private GameObject _goalStockContentItem;
    [SerializeField] private GeneralEvent _tradeGoalConfirmed;

    [Header("Amount selection")]
    [SerializeField] private GameObject _amountSelectionPanel;
    [SerializeField] private Slider _amountSelectionSlider;
    [SerializeField] private GeneralEvent _tradeItemStockChanged;
    [SerializeField] private TradeStockItemButton _tradeStockItemButton;
    [SerializeField] private TextMeshProUGUI _amount;

    private Dictionary<string, Sprite> _sprites;


    private void Awake()
    {
        //create dictionary of pictures
        _sprites = new Dictionary<string, Sprite>();

        foreach (var sprite in Resources.LoadAll<Sprite>("Textures"))
        {
            _sprites.Add(sprite.name, sprite);
        }

        _goalPanelGroup = _goalPanel.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Prepare everything when the trade is initiated
    /// </summary>
    /// <param name="args">TradeStockEventArgs arguments</param>
    public void OnTradeInitiated(EventArgs args)
    {
        TradeStockEventArgs argsStock = (TradeStockEventArgs)args;
        Debug.Log($"Panel is: {_goalPanel.activeInHierarchy}");

        _goalPanel.SetActive(true);

        Transform[] children = _goalContent.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.parent == _goalContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (var stockItem in argsStock.StockItems)
        {
            GameObject contentItem = Instantiate(_goalStockContentItem, _goalContent.transform);
            contentItem.GetComponent<TradeStockItemButton>().SetStockItem(stockItem, OnTradeGoalAmountSelection);

            Sprite sprite = _sprites[stockItem.ItemData.SpriteName];

            if (sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }

        _goalPanelGroup.interactable = true;
    }

    /// <summary>
    /// Once the goal are confirmed, raise TradeGoalConfirmed general event
    /// </summary>
    public void OnButtonGoalConfirmed()
    {
        _goalPanel.SetActive(false);
        _tradeGoalConfirmed.Raise();
    }


    public void OnSliderAmountChanged()
    {
        _amount.text = _amountSelectionSlider.value.ToString();
    }

    public void OnTradeGoalAmountSelection(TradeStockItemButton stockItemButton)
    {
        _goalPanelGroup.interactable = false;
        _amountSelectionPanel.SetActive(true);
        _tradeStockItemButton = stockItemButton;
        _amountSelectionSlider.minValue = 1;
        _amountSelectionSlider.maxValue = _tradeStockItemButton.GetStockItem().Amount;
    }

    public void OnButtonTradeStockItemAmountConfirmed()
    {
        StockItem selectedStockItem = _tradeStockItemButton.GetStockItem();

        StockItem stockItem = new StockItem
        {
            ItemData = selectedStockItem.ItemData,
            Amount = _amountSelectionSlider.value,
            ItemQuality = selectedStockItem.ItemQuality,
            ItemRarity = selectedStockItem.ItemRarity
        };

        _tradeItemStockChanged.Raise(new TradeStockItemEventArgs(stockItem));
        _tradeStockItemButton.SetAmountSelected((int)_amountSelectionSlider.value);
        _amountSelectionPanel.SetActive(false);
        _goalPanelGroup.interactable = true;
    }

    public void OnTradeButtonBack()
    {
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }

    public void OnAmountButtonBack()
    {
        _amountSelectionPanel.SetActive(false);
        _goalPanelGroup.interactable = true;
    }
}
