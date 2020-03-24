using System;
using Terra.Services;
using UnityEngine;

namespace Terra.SerializedData.Entities
{
    public class TerraGridPositionSerializer : IDBSerializer<TerraGridPosition>
    {
        public const string TABLE = "TerraGridPosition";
        
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
                    new IDBColumn() {ColumnName = TerraEntitySerializer.COLUMN_INSTNACE_ID, DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "x", DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "y", DataType = DBDataType.INTEGER}
                };
            }
        }

        public int PrimaryKeyColumnIndex
        {
            get { return 0; }
        }

        public TerraGridPosition Instantiate()
        {
            return new TerraGridPosition();
        }

        public void ParseStringResult(ref TerraGridPosition serializable, int columnIndex, string value)
        {
            throw new System.NotImplementedException();
        }

        public void ParseIntegerResult(ref TerraGridPosition serializable, int columnIndex, int value)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    serializable.InstanceId = value;
                    break;
                }
                case 1:
                {
                    serializable.x = value;
                    break;
                }
                case 2:
                {
                    serializable.y = value;
                    break;
                }
                default:
                    throw new ArgumentException();
            }
        }
        
        public void ParseNumericResult(ref TerraGridPosition serializable, int columnIndex, float value)
        {
            throw new ArgumentException();
        }

        public string GetValue(TerraGridPosition serializable, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    return serializable.InstanceId.ToString();
                }
                case 1:
                {
                    return serializable.x.ToString();
                }
                case 2:
                {
                    return serializable.y.ToString();
                }
            }

            throw new ArgumentException();
        }
    }

    public class TerraGridPosition : IDBSerializable
    {
        public static TerraGridPositionSerializer Serializer { get; } = new TerraGridPositionSerializer();
        public static TerraDBService.DBPrimaryKeyWhereClause<TerraGridPosition, TerraGridPositionSerializer> WherePrimaryKey
        {
            get;
        } = new TerraDBService.DBPrimaryKeyWhereClause<TerraGridPosition, TerraGridPositionSerializer>(Serializer);
        
        public int InstanceId { get; set; }
        public int x { get; set; }
        public int y { get; set; }

        public static bool operator ==(TerraGridPosition a, TerraGridPosition b)
        {
            return a.x == b.x && a.y == b.y && a.InstanceId == b.InstanceId;
        }

        public override bool Equals(object obj)
        {
            if (obj is TerraGridPosition)
            {
                return obj as TerraGridPosition == this;
            }

            return base.Equals(obj);
        }

        public static bool operator !=(TerraGridPosition a, TerraGridPosition b)
        {
            return a != b;
        }

        public TerraGridPosition Set(TerraVector vector)
        {
            x = vector.x;
            y = vector.y;
            return this;
        }
        
        public static implicit operator TerraVector(TerraGridPosition unityVector)
        {
            return new TerraVector()
            {
                x = unityVector.x, y = unityVector.y
            };
        }
    }
}