using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MerchantSelectionUI : MonoBehaviour
{

    [Header("Informations")]
    [SerializeField] private TextMeshProUGUI _merchantName;

    [Header("Stock")]
    [SerializeField] private GameObject _stockContentParent;
    [SerializeField] private GameObject _stockContentItemPrefab;
    [SerializeField] private GameObject _merchantPictureObject;

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


    public void OnMerchantSelected(EventArgs args)
    {
        MerchantEventArgs merchantEventArgs = args as MerchantEventArgs;
        MerchantData merchantData = merchantEventArgs.Merchant.MerchantData;

        //merchant pictrue
        _merchantPictureObject.GetComponent<Image>().sprite = _sprites[merchantData.SpriteName];

        //merchant info
        _merchantName.text = merchantData.Name;

        //stock
        //remove all previous objects
        Transform[] children = _stockContentParent.transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.parent == _stockContentParent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
        }

        foreach (var stockItem in merchantEventArgs.Merchant.StockItems)
        {
            GameObject contentItem = Instantiate(_stockContentItemPrefab, _stockContentParent.transform);

            Sprite sprite = _sprites[stockItem.ItemData.SpriteName];

            if (sprite == null)
            {
                Debug.LogError($"Sprite {stockItem.ItemData.SpriteName} was not found");
            }

            contentItem.GetComponent<Image>().sprite = sprite;
        }

    }
}
