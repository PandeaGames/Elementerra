using System;
using System.Collections.Generic;

namespace Terra.ViewModels
{
    public class TerraGrassNodeGridPoint : GridDataPoint<TerraGrassNode>
    {
        
    }
    
    public struct TerraGrassNode
    {
        public int Grass;
        public float Scale;
    }
    
    public class TerraGrassViewModel : AbstractGridDataModel<TerraGrassNode, TerraGrassNodeGridPoint>
    {
        private TerraGrassPotentialViewModel _terraGrassPotentialViewModel;
        private TerraTerrainGeometryDataModel _terrainModel;
        public TerraGrassViewModel(TerraTerrainGeometryDataModel terrainModel, TerraGrassPotentialViewModel terraGrassPotentialViewModel) : base(new TerraGrassNode[terrainModel.Height,terrainModel.Width])
        {
            _terraGrassPotentialViewModel = terraGrassPotentialViewModel;
            _terrainModel = terrainModel;
            terraGrassPotentialViewModel.OnDataHasChanged += TerraGrassPotentialViewModelOnDataHasChanged;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    TerraVector vector = new TerraVector(x, y);
                    //Random rand = new Random(vector.GetHashCode());
                    this[vector] = CalculateNode(vector, _terraGrassPotentialViewModel);
                }
            }
        }

        private TerraGrassNode CalculateNode(TerraVector vector, TerraGrassPotentialViewModel terraGrassPotentialViewModel)
        {
            return new TerraGrassNode()
            {
                Scale = terraGrassPotentialViewModel[vector],
                Grass = (int) (10 * terraGrassPotentialViewModel[vector])
                //Grass = rand.Next(0, 10)
            };
        }

        private void TerraGrassPotentialViewModelOnDataHasChanged(IEnumerable<TerraGrassPotentialNodeGridPoint> data)
        {
            foreach (TerraGrassPotentialNodeGridPoint dataPoint in data)
            {
                this[dataPoint.Vector] = CalculateNode(dataPoint.Vector, _terraGrassPotentialViewModel);
            }
        }
    }
}