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
    [Header("Goals")]
    [SerializeField] private GameObject _stockPanel;
    private CanvasGroup _panelGroup;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _stockItem;
    [SerializeField] private GeneralEvent _confirmedEvent;

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

        _panelGroup = _stockPanel.GetComponent<CanvasGroup>();
    }

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

            Sprite sprite = _sprites[stockItem.ItemData.SpriteName];

            if (sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }

        _panelGroup.interactable = true;
    }

    public void OnButtonConfirmed()
    {
        _stockPanel.SetActive(false);
        _confirmedEvent.Raise();
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
        {
            ItemData = selectedStockItem.ItemData,
            Amount = _amountSelectionSlider.value,
            ItemQuality = selectedStockItem.ItemQuality,
            ItemRarity = selectedStockItem.ItemRarity
        };

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
