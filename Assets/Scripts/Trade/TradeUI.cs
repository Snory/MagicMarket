using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    private Dictionary<string, Sprite> _sprites;
    [SerializeField] private TextMeshProUGUI _finishButton;
   

    //this is quite brutal, there should be one place to load all the pictures
    private void Awake()
    {
        //create dictionary of pictures
        _sprites = new Dictionary<string, Sprite>();

        foreach (var sprite in Resources.LoadAll<Sprite>("Textures"))
        {
            _sprites.Add(sprite.name, sprite);
        }
    }

    public Sprite GetSprite(string name)
    {
        return _sprites[name];
    }

    public void OnNegotiationPointsChanged(EventArgs args)
    {
        NegotiationPointsEventArgs negotiationPointsEventArgs = (NegotiationPointsEventArgs)args;

        if(negotiationPointsEventArgs.NegotiationPoints > 0)
        {
            _finishButton.text = "Finish it!";
        } else
        {
            _finishButton.text = "Give up already!";
        }

    }

}

