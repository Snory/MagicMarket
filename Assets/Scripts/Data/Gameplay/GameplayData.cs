using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;


[Serializable]
[JsonConverter(typeof(GameplayDataJsonConverter))]
public class GameplayData : Identity
{
    public GameData GameData;  
}
