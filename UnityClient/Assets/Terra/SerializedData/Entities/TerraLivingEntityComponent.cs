using Terra.MonoViews.AI;
using Terra.Services;

namespace Terra.SerializedData.Entities
{
    public class TerraLivingEntityComponent : AbstractEntityComponent<TerraLivingEntity>
    {
        public TerraLivingEntityComponent(TerraDBService DB, TerraLivingEntity Data) : base(DB, Data == null ? new TerraLivingEntity() : Data)
        {
        }

        public void Attack(AttackDef def)
        {
            Data.HP += def.Damage;
            OnChange();
        }

        public override EntityComponent Type { get; }
        protected override IDBSerializer<TerraLivingEntity> Serializer
        {
            get => TerraLivingEntitySerializer.Instance;
        }
        
        protected override TerraDBService.IDBWhereClause<TerraLivingEntity> WhereClause
        {
            get => TerraLivingEntity.WherePrimaryKey;
        }

        public int State
        {
            get { return Data.State; }
            set
            {
                if (Data.State != value)
                {
                    Data.State = value;
                    OnChange();
                }
            }
        }
    }
}