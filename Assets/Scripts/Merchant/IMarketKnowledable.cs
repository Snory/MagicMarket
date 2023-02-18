public interface IMarketKnowledable
{
    public void UpdateStockItemKnowledge(StockItem stockItem, float value);

    public StockItemMarketKnowledge GetItemMarketKnowledge(StockItemBase stockItem);
}