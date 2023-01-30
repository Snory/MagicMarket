using System;

class GameDataEventArgs : EventArgs
{
    public GameData GameData;

    public GameDataEventArgs(GameData gameData)
    {
        GameData = gameData;
    }
}