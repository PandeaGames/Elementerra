namespace Terra.Inventory
{
    public interface IInventoryItem : IInventoryItemData
    {
        InventoryItemDataSerializable Serializable { get; }
    }
}