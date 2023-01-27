using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MerchantManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _merchantPrefab;
    private List<Merchant> _merchants;
    private GameplayData _gamePlayData;
    public MerchantDataFileRepository MerchantDataRepository;
    public ItemDataFileRepository ItemDataRepository;

    public void OnNewGameplay(EventArgs eventArgs)
    {        
        GameplayDataEventArgs gameplayLoaded = eventArgs as GameplayDataEventArgs;
        _gamePlayData = gameplayLoaded.GameplayData;

        for(int i = 0; i < 12; i ++)
        {
            GameObject newMerchantObject = Instantiate(_merchantPrefab, this.transform.position, this.transform.rotation, this.transform);
            Merchant newMerchant = new Merchant(MerchantDataRepository.GetEntry("976a1e77-f36c-4510-8084-c3a90220b50e"), new List<StockItem>());
            MerchantStock merchantStock = newMerchantObject.GetComponent<MerchantStock>();
            merchantStock.OnGameplayLoaded(newMerchant.MerchantStockItems);
            _gamePlayData.Merchants.Add(newMerchant);
        }

        _merchants = _gamePlayData.Merchants;
    }

    public void OnLoadedGameplay(EventArgs eventArgs)
    {
        GameplayDataEventArgs gameplayLoaded = eventArgs as GameplayDataEventArgs;
        _gamePlayData = gameplayLoaded.GameplayData;

        foreach (var merchant in _gamePlayData.Merchants)
        {
            GameObject newMerchantObject = Instantiate(_merchantPrefab, this.transform.position, this.transform.rotation, this.transform);
            MerchantStock merchantStock = newMerchantObject.GetComponent<MerchantStock>();
            merchantStock.OnGameplayLoaded(merchant.MerchantStockItems);
            _merchants.Add(merchant);
        }
    }

}
