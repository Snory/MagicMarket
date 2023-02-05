using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TradeUI : MonoBehaviour
{
    [SerializeField] private GameObject _amountSelectionPanel;
    [SerializeField] private CanvasGroup _panelGroup;


    public void OnTradeButtonBack()
    {
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }

}

