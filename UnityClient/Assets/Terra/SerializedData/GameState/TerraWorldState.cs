using System;
using Terra.Services;

namespace Terra.SerializedData.World
{
    public struct TerraWorldState : IDBSerializable
    {
        public int Tick;
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
                    new IDBColumn() {ColumnName = "tick", DataType = DBDataType.INTEGER}
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
            }

            throw new ArgumentException();
        }
    }
}