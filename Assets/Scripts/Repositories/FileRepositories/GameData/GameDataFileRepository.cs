using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameDataRepository", menuName = "Repository/FileRepository/GameData")]
public class GameDataFileRepository : FileRepository<GameData>
{
    public override void CreateEntry()
    {
        GameData gameData = new GameData();
        gameData.Merchants = new List<Merchant>();
        gameData.Market = new Market();
        gameData.Player = new Player();
        _entries.Add(gameData);

    }
}
