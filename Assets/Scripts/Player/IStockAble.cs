public interface IStockable
{
    public void AddStockItem(StockItem item);
    public void RemoveStockItem(StockItem item);

    public void UpdateTadePower(int value);
}