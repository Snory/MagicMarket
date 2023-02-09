using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _gameDataGuid;

    [SerializeField]
    private GameData _gameData;

    [SerializeField]
    private GeneralEvent _gameDataSent;

    public Repository<GameData> GameDataRepository;
    public Repository<MerchantData> MerchantRepository;
    public Repository<ItemData> ItemRepository;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(_gameDataSent != null)
        {
            _gameDataSent.Raise(new GameDataEventArgs(_gameData));
        }
    }

    [ContextMenu("New game")]
    public void NewGame()
    {
        _gameData = new GameData();

        //load merchant and set their base data
        //load player and set their base data
        //load stock and set their base data

    }


    [ContextMenu("Load game")]
    public void LoadGame()
    {
        SetGameData(GameDataRepository.GetEntry(_gameDataGuid));
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }

    [ContextMenu("Save game")]
    public void SaveGame()
    {
        GameDataRepository.Persist();
    }

    public void SetGameData(GameData data)
    {
        _gameData = data;

        //load Merchant data
        foreach(var merchant in data.Merchants)
        {
            merchant.MerchantData = MerchantRepository.GetEntry(merchant.MerchantData.Identification);
            
            foreach(var stockitem in merchant.StockItems)
            {
                stockitem.ItemData = ItemRepository.GetEntry(stockitem.ItemData.Identification);
            }

            foreach (var itemMarketKnowledge in merchant.ItemMarketKnowledge)
            {
                itemMarketKnowledge.ItemData = ItemRepository.GetEntry(itemMarketKnowledge.ItemData.Identification);
            }
        }

        //load player data
        foreach (var stockitem in _gameData.Player.StockItems)
        {
            stockitem.ItemData = ItemRepository.GetEntry(stockitem.ItemData.Identification);
        }


        //load market stock data
        foreach (var stockitem in _gameData.Market.StockItems)
        {
            stockitem.ItemData = ItemRepository.GetEntry(stockitem.ItemData.Identification);
        }

    }



}
