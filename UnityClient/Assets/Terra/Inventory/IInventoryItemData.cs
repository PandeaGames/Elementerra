namespace Terra.Inventory
{
    public interface IInventoryItemData
    {
        bool IsStackable { get; }
        string Id { get; }
    }
}