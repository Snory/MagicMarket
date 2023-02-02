using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "TradeSessionData", menuName = "SessionData/Trade")]
public class TradeSessionData : ScriptableObject
{
    public Player Player;
    public Merchant Merchant;
    public List<StockItem> StockToBuy;
    public List<StockItem> StockToSell;
    public bool PlayerBuying;


}
