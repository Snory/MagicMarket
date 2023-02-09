using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
[JsonConverter(typeof(GameDataJsonConverter))]
public class GameData : Identity
{
    public string Name;
    public List<Merchant> Merchants;
    public Player Player;
    public Market Market;
}
