using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using static UnityEditor.Progress;

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
        GameData gameData = new GameData();
        gameData.Name = $"NewGame_1";

        Debug.Log("New game");

        GenerateMerchants(gameData);
        GeneratePlayer(gameData);
        GenerateMarket(gameData);
        GameDataRepository.AddEntry(gameData);
        SetGameData(gameData);


        StartGame();
    }

    private void GenerateMarket(GameData gameData)
    {

        Market market = new Market();
        market.StockItems = new List<StockItem>();

        foreach (var merchant in gameData.Merchants)
        {
            foreach (var item in merchant.StockItems)
            {
                market.AddStockItem(item);
            }
        }

        foreach (var item in gameData.Player.StockItems)
        {
            market.AddStockItem(item);
        }

        foreach (var merchant in gameData.Merchants)
        {
            merchant.UpdateGeneralMarketKnowledge();
        }

        gameData.Market = market;
    }

    private void GeneratePlayer(GameData gameData)
    {
        Player player = new Player();
        ItemData data = ItemRepository.GetEntries()[Random.Range(0, ItemRepository.GetEntries().Count)];
        float amount = Random.Range(1, 100);
        float unitTradePower = Random.Range(1, 100);
        StockItem item = new StockItem
        (
            data,
            ItemQuality.LOW,
            ItemRarity.LOW,
            amount,
            unitTradePower,
            amount * unitTradePower
        );
        player.StockItems = new List<StockItem>();
        player.AddStockItem(item);
        gameData.Player = player;
    }

    private void GenerateMerchants(GameData gameData)
    {
        gameData.Merchants = new List<Merchant>();

        //create merchants and their items
        foreach (var merchantData in MerchantRepository.GetEntries())
        {
            Merchant merchant = new Merchant();
            merchant.MerchantData = merchantData;
            ItemData data = ItemRepository.GetEntries()[Random.Range(0, ItemRepository.GetEntries().Count)];
            merchant.StockItems = new List<StockItem>();
            float amount = Random.Range(1, 100);
            float unitTradePower = Random.Range(1, 100);
            StockItem item = new StockItem
            (
                data,
                ItemQuality.LOW,
                ItemRarity.LOW,
                amount,
                unitTradePower,
                amount * unitTradePower
            );
            merchant.AddStockItem(item);

            merchant.ItemMarketKnowledge = new List<StockItemMarketKnowledge>();
            merchant.ItemMarketKnowledge.Add(new StockItemMarketKnowledge
            (
                data,
                ItemQuality.LOW,
                ItemRarity.LOW,
                unitTradePower
            ));
            gameData.Merchants.Add(merchant);
        }
    }

    private void StartGame()
    {
        SceneManager.LoadScene("MarketSelection", LoadSceneMode.Single);
    }

    [ContextMenu("Load game")]
    public void LoadGame()
    {
        SetGameData(GameDataRepository.GetEntry(_gameDataGuid));
        StartGame();
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

        foreach (var transaction in _gameData.Market.StockItemsTransactions)
        {
            foreach(var stockItemSold in transaction.StockItemsSold)
            {
                stockItemSold.ItemData = ItemRepository.GetEntry(stockItemSold.ItemData.Identification);
            }

            foreach (var stockItemBought in transaction.StockItemsBought)
            {
                stockItemBought.ItemData = ItemRepository.GetEntry(stockItemBought.ItemData.Identification);
            }

            transaction.MerchantData = MerchantRepository.GetEntry(transaction.MerchantData.Identification);
        }


    }



}
