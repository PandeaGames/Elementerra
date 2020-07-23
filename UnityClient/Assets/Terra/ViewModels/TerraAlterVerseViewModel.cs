using System;
using System.Collections.Generic;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.SerializedData.World;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraAlterVerseGridPoint : GridDataPoint<bool>
    {
        public TerraAlterVerseGridPoint() : base()
        {
        }

        public TerraAlterVerseGridPoint(TerraVector vector, bool value) : base(value, vector)
        {
            
        }
    }
    
    public class TerraAlterVerseViewModel : AbstractGridDataModel<bool, TerraAlterVerseGridPoint>
    {
        private TerraWorldChunk _chunk;
        public TerraAlterVerseViewModel(TerraEntitiesViewModel entitiesModel, TerraWorldChunk chunk) : base(new bool[chunk.Height,chunk.Width])
        {
            _chunk = chunk;
            entitiesModel.OnAddEntity += EntitiesModelOnAddEntity;

            foreach (RuntimeTerraEntity entity in entitiesModel)
            {
                EntitiesModelOnAddEntity(entity);
            }
        }
        
        private void EntitiesModelOnAddEntity(RuntimeTerraEntity entity)
        {
            List<TerraAlterVerseGridPoint> changes = new List<TerraAlterVerseGridPoint>(ProcessEntity(entity, _chunk));
            if(changes.Count > 0)
                DataHasChanged(changes);
        }

        private IEnumerable<TerraAlterVerseGridPoint> ProcessEntity(RuntimeTerraEntity entity, TerraWorldChunk chunk)
        {
            TerraEntityTypeData typeData = entity.EntityTypeData;
            if (!typeData.IsUniverseGateway) yield break;
            TerraVector localVector = chunk.WorldToLocal(entity.GridPosition.Data);
            for (int x = Math.Max(0, localVector.x - typeData.UniverseGatewayRadius);
                x < Math.Min(Width, localVector.x + typeData.UniverseGatewayRadius);
                x++)
            {
                for (int y = Math.Max(0, localVector.y - typeData.UniverseGatewayRadius);
                    y < Math.Min(Width, localVector.y + typeData.UniverseGatewayRadius);
                    y++)
                {
                    TerraVector vector = new TerraVector(x, y);
                    float dx = x - localVector.x;
                    float dy = y - localVector.y;
                    float d = Mathf.Sqrt(dx * dx + dy * dy);
                    this[vector] = d < typeData.UniverseGatewayRadius;
                    //if within radius, true. otherwise, false
                    yield return new TerraAlterVerseGridPoint(vector, this[vector]);
                }
            }
            
        }
    }
}