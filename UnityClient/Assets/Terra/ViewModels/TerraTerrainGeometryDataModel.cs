using System.Collections.Generic;
using Terra.SerializedData.World;
using Terra.Services;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraTerrainGeometryDataPoint : GridDataPoint<Vector3>
    {
        public TerraTerrainGeometryDataPoint()
        {
            
        }
        public TerraTerrainGeometryDataPoint(TerraVector vector, Vector3 data) : base(data, vector)
        {
            
        }
    }
    
    public class TerraTerrainGeometryDataModel : AbstractGridDataModel<Vector3, TerraTerrainGeometryDataPoint>
    {
        private TerraWorldChunk _chunk;
        public TerraTerrainGeometryDataModel(TerraWorldChunk chunk) : base(new Vector3[chunk.Width,chunk.Height])
        {
            _chunk = chunk;
                
            //Create Vertices
            for (int x = 0; x < chunk.Width; x++)
            {
                for (int y = 0; y < chunk.Height; y++)
                {
                    this[x, y] = GetVertice(chunk, x, y);
                }
            }
            
            chunk.OnDataHasChanged += TerraWorldChunkOnDataHasChanged;
        }

        private void TerraWorldChunkOnDataHasChanged(IEnumerable<TerraDataPoint> data)
        {
            _isBatchingChanges = true;
            Dictionary<TerraVector, TerraTerrainGeometryDataPoint> changes = new Dictionary<TerraVector, TerraTerrainGeometryDataPoint>();
            foreach (TerraDataPoint dataPoint in data)
            {
                UpdateSurroundingHights(dataPoint.Vector, _chunk);

                foreach (TerraTerrainGeometryDataPoint terraTerrainGeometryDataPoint in OnDataChange(dataPoint.Vector))
                {
                    if (!changes.ContainsKey(terraTerrainGeometryDataPoint.Vector))
                    {
                        changes.Add(
                            terraTerrainGeometryDataPoint.Vector, 
                            new TerraTerrainGeometryDataPoint(
                                terraTerrainGeometryDataPoint.Vector,
                                this[terraTerrainGeometryDataPoint.Vector]));
                    }
                }
            }
            _isBatchingChanges = false;
            DataHasChanged(changes.Values);
        }
        
        private void UpdateSurroundingHights(TerraVector vector, TerraWorldChunk chunk)
        {
            for (int x = vector.x - 1; x <= vector.x + 1; x++)
            {
                for (int y = vector.y - 1; y <= vector.y + 1; y++)
                {
                    this[x, y] = GetVertice(chunk, x, y);
                }
            }
        }

        private IEnumerable<TerraTerrainGeometryDataPoint> OnDataChange(TerraVector vector)
        {
            for (int x = vector.x - 1; x <= vector.x + 1; x++)
            {
                for (int y = vector.y - 1; y <= vector.y + 1; y++)
                {
                    yield return new TerraTerrainGeometryDataPoint(new TerraVector(x, y), this[x, y]);
                }
            }
        }
        
        private Vector3 GetVertice(TerraWorldChunk chunk, int x, int y)
        {
            if (x < 1 || x > chunk.Width - 1 || y < 1 || y > chunk.Height - 1)
            {
                return new Vector3(chunk[x, y].Position.x, chunk[x, y].Height * 0.1f, chunk[x, y].Position.y);
            }
            return new Vector3(
                       chunk[x, y].Position.x, 
                       ((chunk[x, y].Height +
                        chunk[x - 1, y -1].Height +
                        chunk[x, y -1].Height +
                        chunk[x + 1, y -1].Height +
                        chunk[x + 1, y].Height +
                        chunk[x + 1, y + 1].Height +
                        chunk[x, y + 1].Height +
                        chunk[x - 1, y + 1].Height +
                        chunk[x - 1, y].Height) / 9f) * 0.1f,
            chunk[x, y].Position.y);
        }

        public Vector3 TryGetClosestGridPosition(Vector3 worldPosition)
        {
            TerraVector terraVector = new TerraVector((int)worldPosition.x, (int)worldPosition.z);
            return TryGetClosestGridPosition(terraVector);
        }
        
        public Vector3 TryGetClosestGridPosition(TerraVector terraVector)
        {
            Vector3 position = new Vector3(terraVector.x, 0, terraVector.y);

            if (terraVector.x > _chunk[0, 0].Position.x && 
                terraVector.x < _chunk[0, 0].Position.x + Width &&
                terraVector.y > _chunk[0, 0].Position.y && 
                terraVector.y < _chunk[0, 0].Position.y + Height)
            {
                position = this[terraVector.x - _chunk[0, 0].Position.x, terraVector.y - _chunk[0, 0].Position.y];
            }

            return position;
        }
    }
}