using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MerchantSelector : MonoBehaviour
{
    [SerializeField] private List<Merchant> _merchants;
    [SerializeField] private int _currentMerchantIndex;
    [SerializeField] GeneralEvent _merchantSelected;

    private void Awake()
    {
        _currentMerchantIndex = -1;
    }

    public void OnSelectMerchant(int direction)
    {
        if(_merchants == null)
        {
            Debug.LogError("Merchant were not laoded");
        }

        switch (direction)
        {
            case -1:
                if(_currentMerchantIndex == 0)
                {
                    _currentMerchantIndex = _merchants.Count - 1;
                } else
                {
                    _currentMerchantIndex -= 1;
                }
                break;
            case 1:
                if (_currentMerchantIndex == _merchants.Count - 1)
                {
                    _currentMerchantIndex = 0;
                }
                else
                {
                    _currentMerchantIndex += 1;
                }
                break;
        }

        RaiseMerchantSelected();
    }

    public void RaiseMerchantSelected()
    {
        _merchantSelected.Raise(new MerchantEventArgs(_merchants[_currentMerchantIndex]));
    }

    public void OnGameDataSent(EventArgs args)
    {
        GameDataEventArgs gameData = args as GameDataEventArgs;
        _merchants = gameData.GameData.Merchants;

        //after scene is loaded and data are sent
        if(_currentMerchantIndex == -1)
        {
            _currentMerchantIndex = 0;
            RaiseMerchantSelected();
        }

    }

}
