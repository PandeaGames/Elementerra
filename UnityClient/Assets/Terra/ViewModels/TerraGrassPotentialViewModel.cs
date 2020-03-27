using System;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.SerializedData.World;
using Terra.Services;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraGrassPotentialNodeGridPoint : GridDataPoint<float>
    {
        
    }
    
    public struct TerraGrassPotentialNode
    {
        public float GrassPotential;
    }
    
    public class TerraGrassPotentialViewModel : AbstractGridDataModel<float, TerraGrassPotentialNodeGridPoint>
    {
        private TerraWorldChunk _chunk;
        public TerraGrassPotentialViewModel(TerraTerrainGeometryDataModel terrainModel, TerraEntitiesViewModel entitiesModel, TerraWorldChunk chunk) : base(new float[terrainModel.Height,terrainModel.Width])
        {
            _chunk = chunk;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    this[x, y] = 1;
                    ProcessGrassBasedOnGeometry(x, y, terrainModel);
                }
            }
            
            entitiesModel.OnAddEntity += EntitiesModelOnAddEntity;

            foreach (RuntimeTerraEntity entity in entitiesModel)
            {
                EntitiesModelOnAddEntity(entity);
            }
        }

        private void ProcessGrassBasedOnGeometry(int xIn, int yIn, TerraTerrainGeometryDataModel terrainModel)
        {
            float variation = 0;
            int r = 1;
            for (int x = Math.Max(0, xIn - r);
                x < Math.Min(Width, xIn + r);
                x++)
            {
                for (int y = Math.Max(0, yIn - r);
                    y < Math.Min(Height, yIn + r);
                    y++)
                {
                    variation += Math.Abs(terrainModel[x, y].y - terrainModel[xIn, yIn].y);
                }
            }

            if (variation > 1 || terrainModel[xIn, yIn].y < 0)
            {
                this[xIn, yIn] = 0;
            }
        }

        private void EntitiesModelOnAddEntity(RuntimeTerraEntity entity)
        {
            ProcessEntity(entity, _chunk);
        }

        private void ProcessEntity(RuntimeTerraEntity entity, TerraWorldChunk chunk)
        {
            TerraEntityTypeData typeData = entity.EntityTypeData;
            TerraVector localVector = chunk.WorldToLocal(entity.GridPosition.Data);
            if (typeData.GrassPotentialReductionRadius > 0)
            {
                for (int x = Math.Max(0, localVector.x - typeData.GrassPotentialReductionRadius);
                    x < Math.Min(Width, localVector.x + typeData.GrassPotentialReductionRadius);
                    x++)
                {
                    for (int y = Math.Max(0, localVector.y - typeData.GrassPotentialReductionRadius);
                        y < Math.Min(Width, localVector.y + typeData.GrassPotentialReductionRadius);
                        y++)
                    {
                        float dx = x - localVector.x;
                        float dy = y - localVector.y;
                        float d = Mathf.Sqrt(dx * dx + dy * dy);
                        float factor = d / (float) typeData.GrassPotentialReductionRadius;
                        float reducedGrassPotential = typeData.GrassPotentialReductionCurve.Evaluate(factor);
                        this[x, y] = Math.Min(this[x, y], reducedGrassPotential);
                        //DataHasChanged();
                    }
                }
            }
        }
    }
}