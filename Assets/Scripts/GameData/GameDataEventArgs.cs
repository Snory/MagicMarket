using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataEventArgs : EventArgs
{
    public GameData GameData;
    public GameDataEventArgs(GameData gameData)
    {
        GameData = gameData;
    }
}
