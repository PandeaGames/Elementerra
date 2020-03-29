using System;
using Terra.Services;

namespace Terra.SerializedData.GameState
{
    public struct TerraPlayerState : IDBSerializable
    {
        public string HoldingEntityID;
    }
    
    public class TerraPlayerStateSerializer : IDBSerializer<TerraPlayerState>
    {
        public const string TABLE = "PlayerState";
        
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
                    new IDBColumn() {ColumnName = "holdingEntityID", DataType = DBDataType.TEXT}
                };
            }
        }

        public int PrimaryKeyColumnIndex
        {
            get { return -1; }
        }

        public TerraPlayerState Instantiate()
        {
            return new TerraPlayerState();
        }

        public void ParseStringResult(ref TerraPlayerState serializable, int columnIndex, string value)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    serializable.HoldingEntityID = value;
                    break;
                }
            }
        }

        public void ParseIntegerResult(ref TerraPlayerState serializable, int columnIndex, int value)
        {
            
        }
        
        public void ParseNumericResult(ref TerraPlayerState serializable, int columnIndex, float value)
        {
            throw new ArgumentException();
        }

        public string GetValue(TerraPlayerState serializable, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    return serializable.HoldingEntityID;
                }
            }

            throw new ArgumentException();
        }
    }
}