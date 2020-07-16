using System;
using Terra.Services;

namespace Terra.SerializedData.World
{
    public struct TerraWorldState : IDBSerializable
    {
        public int Tick;
        public int WorldFlipped;

        public bool IsWorldFipped
        {
            get
            {
                return WorldFlipped == 1;
            }
            set
            {
                WorldFlipped = value ? 1:0;
            }
        }

        public override int GetHashCode()
        {
            return Tick;
        }
    }
    
    public class TerraWorldStateSerializer : IDBSerializer<TerraWorldState>
    {
        public const string TABLE = "TerraWorldState";
        
        public string Table
        {
            get { return TABLE; }
        }

        public IDBColumn[] Columns
        {
            get
            {
                return new IDBColumn[]
                {
                    new IDBColumn() {ColumnName = "tick", DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "WorldFlipped", DataType = DBDataType.INTEGER}
                };
            }
        }

        public int PrimaryKeyColumnIndex
        {
            get { return 0; }
        }

        public TerraWorldState Instantiate()
        {
            return new TerraWorldState();
        }

        public void ParseStringResult(ref TerraWorldState serializable, int columnIndex, string value)
        {
            throw new System.NotImplementedException();
        }

        public void ParseIntegerResult(ref TerraWorldState serializable, int columnIndex, int value)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    serializable.Tick = value;
                    break;
                }
                case 1:
                {
                    serializable.WorldFlipped = value;
                    break;
                }
            }
        }
        
        public void ParseNumericResult(ref TerraWorldState serializable, int columnIndex, float value)
        {
            throw new ArgumentException();
        }

        public string GetValue(TerraWorldState serializable, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    return serializable.Tick.ToString();
                }
                case 1:
                {
                    return serializable.WorldFlipped.ToString();
                }
            }

            throw new ArgumentException();
        }
    }
}