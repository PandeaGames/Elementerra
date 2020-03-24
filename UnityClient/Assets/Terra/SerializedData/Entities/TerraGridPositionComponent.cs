using System.Collections.Generic;
using System.Collections;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.SerializedData.Entities
{
    public partial class TerraEntities
    {
        public Dictionary<int, TerraGridPosition> GridPositions { get; set; }

        public IEnumerator<TerraGridPosition> GetGridPositionsEnumerator()
        {
            foreach (KeyValuePair<int, TerraGridPosition> kvp in GridPositions)
            {
                yield return kvp.Value;
            }
        }
    }
    
    public class TerraGridPositionComponent : AbstractEntityComponent<TerraGridPosition>
    {
        public override EntityComponent Type
        {
            get { return EntityComponent.GridPosition; }
        }

        protected override IDBSerializer<TerraGridPosition> Serializer { get; } = TerraGridPosition.Serializer;
        protected override TerraDBService.IDBWhereClause<TerraGridPosition> WhereClause { get; } = TerraGridPosition.WherePrimaryKey;
        
        public TerraGridPositionComponent(TerraDBService DB, TerraGridPosition Data) : base(DB, Data)
        {
        }
        
        public TerraGridPosition Set(TerraVector position)
        {
            bool didChange = !position.Equals(Data);
            Data.Set(position);

            if(didChange)
                OnChange();
            
            return Data;
        }
    }
}