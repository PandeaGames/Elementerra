namespace Terra.Inventory
{
    public class InventoryItem : IInventoryItem
    {
        private InventoryItemDataSerializable _serializable;
        private IInventoryItemData _inventoryItemData;

        public InventoryItemDataSerializable Serializable
        {
            get => _serializable;
        }
        
        public bool IsStackable
        {
            get => _inventoryItemData.IsStackable;
        }
        
        public string Id
        {
            get => _inventoryItemData.Id;
        }

        public InventoryItem(IInventoryItemData inventoryItemData,InventoryItemDataSerializable serializable)
        {
            _inventoryItemData = inventoryItemData;
            _serializable = serializable;
        }
    }
}