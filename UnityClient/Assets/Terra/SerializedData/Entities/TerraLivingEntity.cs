using System;
using PandeaGames;
using Terra.Services;

namespace Terra.SerializedData.Entities
{
    [Serializable]
    public class TerraLivingEntity : IDBSerializable
    {
        public static TerraDBService.DBPrimaryKeyWhereClause<TerraLivingEntity, TerraLivingEntitySerializer> WherePrimaryKey
        {
            get;
        } = new TerraDBService.DBPrimaryKeyWhereClause<TerraLivingEntity, TerraLivingEntitySerializer>(TerraLivingEntitySerializer.Instance);
        
        public int InstanceId;
        public int HP;
        public int State;
        
    }

    public class TerraLivingEntitySerializer : Singleton<TerraLivingEntitySerializer>, IDBSerializer<TerraLivingEntity>
    {
        public string Table => "entityLife";
        
        public IDBColumn[] Columns
        {
            get
            {
                return new IDBColumn[]
                {
                    new IDBColumn() {ColumnName = TerraEntitySerializer.COLUMN_INSTNACE_ID, DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "hp", DataType = DBDataType.INTEGER},
                    new IDBColumn() {ColumnName = "state", DataType = DBDataType.INTEGER}
                };
            }
        }

        public int PrimaryKeyColumnIndex => 0;
        
        public TerraLivingEntity Instantiate()
        {
            return new TerraLivingEntity();
        }

        public void ParseStringResult(ref TerraLivingEntity serializable, int columnIndex, string value)
        {
            throw new NotImplementedException();
        }

        public void ParseIntegerResult(ref TerraLivingEntity serializable, int columnIndex, int value)
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
                    serializable.HP = value;
                    break;
                }
                case 2:
                {
                    serializable.State = value;
                    break;
                }
            }
        }

        public void ParseNumericResult(ref TerraLivingEntity serializable, int columnIndex, float value)
        {
            throw new NotImplementedException();
        }

        public string GetValue(TerraLivingEntity serializable, int columnIndex)
        {
            switch (columnIndex)
            {
                case 0:
                {
                    return serializable.InstanceId.ToString();
                }
                case 1:
                {
                    return serializable.HP.ToString();
                }
                case 2:
                {
                    return serializable.State.ToString();
                }
            }
            
            throw new NotImplementedException();
        }
    }
}