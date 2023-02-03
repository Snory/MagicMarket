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
    [SerializeField] private GameObject _goalContent;
    [SerializeField] private GameObject _goalStockContentItem;
    [SerializeField] private GeneralEvent _tradeGoalConfirmed;
    
    
    private Dictionary<string, Sprite> _sprites;


    private void Awake()
    {
        //create dictionary of pictures
        _sprites = new Dictionary<string, Sprite>();

        foreach (var sprite in Resources.LoadAll<Sprite>("Textures"))
        {
            _sprites.Add(sprite.name, sprite);
        }
    }

    public void OnTradeInitiated(EventArgs args)
    {
        TradeStockEventArgs argsStock = (TradeStockEventArgs) args;
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
            contentItem.GetComponent<TradeStockItemButton>().SetStockItem(stockItem);

            Sprite sprite = _sprites[stockItem.ItemData.SpriteName];

            if (sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }
    }

    public void OnButtonGoalConfirmed()
    {
        _goalPanel.SetActive(false);

        Debug.Log($"Panel is: {_goalPanel.activeInHierarchy}");

        _tradeGoalConfirmed.Raise();
    }

    public void OnButtonBack()
    {
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }
}
