using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayDataEventArgs : EventArgs
{
    public GameplayData GameplayData;

    public GameplayDataEventArgs(GameplayData gameplayData)
    {
        GameplayData = gameplayData;
    }
}
