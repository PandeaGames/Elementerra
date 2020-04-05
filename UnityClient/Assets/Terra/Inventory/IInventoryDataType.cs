namespace Terra.Inventory
{
    public interface IInventoryDataType
    {
        int MaxInventorySize { get; }
        string Id { get; }
        int MaxStackCount { get; }
    }
}