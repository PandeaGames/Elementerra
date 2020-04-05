using PandeaGames;
using Terra.Services;

namespace Terra.Inventory
{
    public class InventoryItemDataSerializer : Singleton<InventoryItemDataSerializer>, IDBSerializer<InventoryItemDataSerializable>
    {
        public string Table
        {
            get => "Inventory";
        }

        public IDBColumn[] Columns
        {
            get => new IDBColumn[]
            {
                new IDBColumn(){ColumnName = "inventoryId", DataType = DBDataType.INTEGER},
                new IDBColumn(){ColumnName = "itemId", DataType = DBDataType.TEXT},
                new IDBColumn(){ColumnName = "stack", DataType = DBDataType.INTEGER},
                new IDBColumn(){ColumnName = "tickAdded", DataType = DBDataType.INTEGER}
            };
        }
        
        public int PrimaryKeyColumnIndex
        {
            get => -1;
        }
        
        public InventoryItemDataSerializable Instantiate()
        {
            return new InventoryItemDataSerializable();
        }

        public void ParseStringResult(ref InventoryItemDataSerializable serializable, int columnIndex, string value)
        {
            switch (columnIndex)
            {
                case 2:
                {
                    serializable.Id = value;
                    break;
                }
                default:
                    throw new System.InvalidCastException();
            }
        }

        public void ParseIntegerResult(ref InventoryItemDataSerializable serializable, int columnIndex, int value)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    serializable.RowId = value;
                    break;
                }
                case 1:
                {
                    serializable.InventoryId = value;
                    break;
                }
                case 2:
                {
                    throw new System.InvalidCastException();
                }
                case 3:
                {
                    serializable.Stack = value;
                    break;
                }
                case 4:
                {
                    serializable.TickAdded = value;
                    break;
                }
                default:
                {
                    throw new System.InvalidCastException();
                }
            }
        }

        public void ParseNumericResult(ref InventoryItemDataSerializable serializable, int columnIndex, float value)
        {
            throw new System.NotImplementedException();
        }

        public string GetValue(InventoryItemDataSerializable serializable, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    return serializable.InventoryId.ToString();
                }
                case 1:
                {
                    return serializable.Id;
                }
                case 2:
                {
                    return serializable.Stack.ToString();
                }
                case 3:
                {
                    return serializable.TickAdded.ToString();
                }
                default:
                {
                    throw new System.InvalidCastException();
                }
            }
        }
    }
}