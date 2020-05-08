using System;
using System.IO;
using Terra.Services;

namespace Terra.SerializedData.GameState
{
    public struct TerraPlayerState : IDBSerializable
    {
        public string HoldingEntityID;
        public int HoldingInstanceID;
        public string HoldingInHandEntityId;
        public int HoldingInHandEntityInstanceId;
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
                    new IDBColumn() {ColumnName = "holdingEntityID", DataType = DBDataType.TEXT},
                    new IDBColumn() {ColumnName = "holdingInstanceID", DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "holdingInHandEntityID", DataType = DBDataType.TEXT},
                    new IDBColumn() {ColumnName = "holdingInHandInstanceID", DataType = DBDataType.INTEGER}
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
                case 2:
                {
                    serializable.HoldingInHandEntityId = value;
                    break;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
        }

        public void ParseIntegerResult(ref TerraPlayerState serializable, int columnIndex, int value)
        {
            switch (columnIndex)
            {
                case 1:
                {
                    serializable.HoldingInstanceID = value;
                    break;
                }
                case 3:
                {
                    serializable.HoldingInHandEntityInstanceId = value;
                    break;
                }
                default:
                {
                    throw new ArgumentException();
                }
            }
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
                case 1:
                {
                    return serializable.HoldingInstanceID.ToString();
                }
                case 2:
                {
                    return serializable.HoldingInHandEntityId;
                }
                case 3:
                {
                    return serializable.HoldingInHandEntityInstanceId.ToString();
                }
            }

            throw new ArgumentException();
        }
    }
}