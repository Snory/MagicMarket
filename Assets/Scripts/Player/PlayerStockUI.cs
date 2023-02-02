using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStockUI : MonoBehaviour
{
    public GameObject StockObject;
    public GameObject Content;
    public GameObject ContentItemPrefab;
    public GameObject CloseButton;
    public GameObject OpenButton;
    private Dictionary<string, Sprite> _sprites;

    private void Awake()
    {
        //create dictionary of pictures
        _sprites = new Dictionary<string, Sprite>();

        foreach(var sprite in Resources.LoadAll<Sprite>("Textures"))
        {
            _sprites.Add(sprite.name, sprite);
        }
    }

    public void OnShowStockUI()
    {
        if (StockObject.activeInHierarchy)
        {
            StockObject.SetActive(false);
            OpenButton.SetActive(true);
            CloseButton.SetActive(false);
        } else
        {
            StockObject.SetActive(true);
            OpenButton.SetActive(false);
            CloseButton.SetActive(true);
        }
    }

    public void OnStockChanged(EventArgs args)
    {
        StockChangedEventArgs stockChanged = args as StockChangedEventArgs;

        //remove all previous objects
        Transform[] children = Content.transform.GetComponentsInChildren<Transform>();
        foreach (Transform child in children)
        {
            if (child.parent == transform)
            {
                GameObject.Destroy(child.GetComponent<GameObject>());
            }
        }

        foreach(var stockItem in stockChanged.StockItems)
        {
            GameObject contentItem = Instantiate(ContentItemPrefab, Content.transform);

            Sprite sprite = _sprites[stockItem.ItemData.SpriteName];

            if(sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }

    }
    

}
