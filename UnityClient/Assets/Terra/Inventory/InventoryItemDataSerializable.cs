using System;
using Terra.Services;

namespace Terra.Inventory
{
    [Serializable]
    public class InventoryItemDataSerializable : IDBSerializable
    {
        public int RowId;
        public int InventoryId;
        public int Stack;
        public string Id;
        public int TickAdded;
    }
}