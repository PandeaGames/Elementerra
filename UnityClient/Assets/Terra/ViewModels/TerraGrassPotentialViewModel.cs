using System;
using System.Collections.Generic;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.SerializedData.World;
using Terra.Services;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraGrassPotentialNodeGridPoint : GridDataPoint<float>
    {
        public TerraGrassPotentialNodeGridPoint() : base()
        {
        }

        public TerraGrassPotentialNodeGridPoint(TerraVector vector, float value) : base(value, vector)
        {
            
        }
    }
    
    public class TerraGrassPotentialViewModel : AbstractGridDataModel<float, TerraGrassPotentialNodeGridPoint>
    {
        private TerraWorldChunk _chunk;
        private TerraTerrainGeometryDataModel _terrainModel;
        private float[,] _cachedReducedGrassPotential;
        public TerraGrassPotentialViewModel(TerraTerrainGeometryDataModel terrainModel, TerraEntitiesViewModel entitiesModel, TerraWorldChunk chunk) : base(new float[terrainModel.Height,terrainModel.Width])
        {
            _cachedReducedGrassPotential = new float[terrainModel.Height,terrainModel.Width];
            _terrainModel = terrainModel;
            
            _chunk = chunk;
            _chunk.OnDataHasChanged += ChunkOnDataHasChanged;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    _cachedReducedGrassPotential[x, y] = 1;
                    this[x, y] = CalculateBaseValue(x, y, terrainModel);
                }
            }
            
            entitiesModel.OnAddEntity += EntitiesModelOnAddEntity;
            terrainModel.OnDataHasChanged += TerrainModelOnDataHasChanged;

            foreach (RuntimeTerraEntity entity in entitiesModel)
            {
                EntitiesModelOnAddEntity(entity);
            }
        }

        private void ChunkOnDataHasChanged(IEnumerable<TerraDataPoint> data)
        {
            _isBatchingChanges = true;
            foreach (TerraDataPoint dataPoint in data)
            {
                this[dataPoint.Vector] = GetFullValue(dataPoint.Vector.x, dataPoint.Vector.y, _cachedReducedGrassPotential[dataPoint.Vector.x, dataPoint.Vector.y]);
            }

            _isBatchingChanges = false;
            DataHasChanged(ReportDataChangeForRange<TerraDataPoint, TerraPoint>(data));
        }

        private void TerrainModelOnDataHasChanged(IEnumerable<TerraTerrainGeometryDataPoint> data)
        {
            _isBatchingChanges = true;
            foreach (TerraTerrainGeometryDataPoint dataPoint in data)
            {
                this[dataPoint.Vector] = GetFullValue(dataPoint.Vector.x, dataPoint.Vector.y, _cachedReducedGrassPotential[dataPoint.Vector.x, dataPoint.Vector.y]);
            }

            _isBatchingChanges = false;
             DataHasChanged(ReportChange(data));
        }

        private IEnumerable<TerraGrassPotentialNodeGridPoint> ReportChange(IEnumerable<TerraTerrainGeometryDataPoint> data)
        {
            foreach (TerraTerrainGeometryDataPoint dataPoint in data)
            {
                yield return new TerraGrassPotentialNodeGridPoint(dataPoint.Vector, this[dataPoint.Vector]);
            }
        }

        private float CalculateBaseValue(int xIn, int yIn, TerraTerrainGeometryDataModel terrainModel)
        {
            float value = Math.Max(0, (float)1 - (float) _chunk[xIn, yIn].Erosion / 50);

            float variation = 0;
            int r = 2;
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
                value = 0;
            }
            
            return value;
        }

        private float GetFullValue(int x, int y,float reducedGrassPotential)
        {
            return Mathf.Min(CalculateBaseValue(x, y, _terrainModel), reducedGrassPotential);
        }

        private void EntitiesModelOnAddEntity(RuntimeTerraEntity entity)
        {
            DataHasChanged(ProcessEntity(entity, _chunk));
        }

        private IEnumerable<TerraGrassPotentialNodeGridPoint> ProcessEntity(RuntimeTerraEntity entity, TerraWorldChunk chunk)
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
                        TerraVector vector = new TerraVector(x, y);
                        float dx = x - localVector.x;
                        float dy = y - localVector.y;
                        float d = Mathf.Sqrt(dx * dx + dy * dy);
                        float factor = d / (float) typeData.GrassPotentialReductionRadius;
                        float reducedGrassPotential = typeData.GrassPotentialReductionCurve.Evaluate(factor);
                        _cachedReducedGrassPotential[x, y] = reducedGrassPotential;
                        this[x, y] = GetFullValue(x, y, reducedGrassPotential);
                        yield return new TerraGrassPotentialNodeGridPoint(vector, this[vector]);
                        //DataHasChanged();
                    }
                }
            }
        }
    }
}