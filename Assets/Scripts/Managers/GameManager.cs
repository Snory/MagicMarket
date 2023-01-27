using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameplayFileRepository _gameplayRepository;

    [SerializeField]
    private GameDataFileRepository _gameDataRepository;

    [SerializeField]
    private GeneralEvent _newGameplay;

    public void OnStartGame()
    {
        GameData gameData = new GameData();

        _gameDataRepository.AddEntry(gameData);

        GameplayData gameplayData = new GameplayData {
            GameData= gameData, Merchants = new List<Merchant>() 
        };

        _gameplayRepository.AddEntry(gameplayData);
        _newGameplay.Raise(new GameplayDataEventArgs(gameplayData));
    }

}
