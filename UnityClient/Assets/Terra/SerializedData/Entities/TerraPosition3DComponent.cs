﻿using System.Collections.Generic;
using System.Collections;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.SerializedData.Entities
{
    public partial class TerraEntities
    {
        public Dictionary<int, TerraPosition3D> Positions { get; set; }

        public IEnumerator<TerraPosition3D> GetPositionsEnumerator()
        {
            foreach (KeyValuePair<int, TerraPosition3D> kvp in Positions)
            {
                yield return kvp.Value;
            }
        }
    }
    
    public class TerraPosition3DComponent : AbstractEntityComponent<TerraPosition3D>
    {
        public override EntityComponent Type
        {
            get { return EntityComponent.Position; }
        }

        protected override IDBSerializer<TerraPosition3D> Serializer { get; } = TerraPosition3D.Serializer;
        protected override TerraDBService.IDBWhereClause<TerraPosition3D> WhereClause { get; } = TerraPosition3D.WherePrimaryKey;
        
        public TerraPosition3DComponent(TerraDBService DB, TerraPosition3D Data) : base(DB, Data)
        {
        }
        
        public TerraPosition3D Set(Quaternion rotation)
        {
            float d = Vector3.Distance(rotation.eulerAngles, Data.euler);

            if (d > 0.001)
            {
                Data.Set(rotation);
                OnChange();
            }
            
            return Data;
        }
        
        public TerraPosition3D Set(Vector3 position)
        {
            Vector3 currentPosition = (Vector3) Data;
            float d = Vector3.Distance(currentPosition, position);
            if (d > 0.01)
            {
                Data.Set(position);
                OnChange();
            }
            
            return Data;
        }
    }
}