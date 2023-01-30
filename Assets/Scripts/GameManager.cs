using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private string _gameDataGuid;

    [SerializeField]
    private GameData _gameData;

    [SerializeField]
    private GeneralEvent _gameDataLoaded;

    public Repository<GameData> GameDataRepository;
    public Repository<MerchantData> MerchantRepository;
    public Repository<ItemData> ItemRepository;


    [ContextMenu("Load game")]
    public void LoadGame()
    {
        SetGameData(GameDataRepository.GetEntry(_gameDataGuid));
    }

    [ContextMenu("Save game")]
    public void SaveGame()
    {
        GameDataRepository.Persist();
    }

    public void SetGameData(GameData data)
    {
        _gameData = data;

        //load merchant data
        foreach(var merchant in data.Merchants)
        {
            merchant.MerchantData = MerchantRepository.GetEntry(merchant.MerchantData.Identification);

            foreach(var stockitem in merchant.StockItems)
            {
                stockitem.ItemData = ItemRepository.GetEntry(stockitem.ItemData.Identification);
            }
        }

        foreach (var stockitem in _gameData.Player.StockItems)
        {
            stockitem.ItemData = ItemRepository.GetEntry(stockitem.ItemData.Identification);
        }

        _gameDataLoaded.Raise(new GameDataEventArgs(_gameData));
    }



}
