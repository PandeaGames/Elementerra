using System;

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
        public TerraGrassViewModel(TerraTerrainGeometryDataModel terrainModel, TerraGrassPotentialViewModel terraGrassPotentialViewModel) : base(new TerraGrassNode[terrainModel.Height,terrainModel.Width])
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    TerraVector vector= new TerraVector(x, y);
                    Random rand = new Random(vector.GetHashCode());
                    this[x, y] = new TerraGrassNode()
                    {
                        Scale = terraGrassPotentialViewModel[x, y],
                        Grass = (int) (10 * terraGrassPotentialViewModel[x, y])
                        //Grass = rand.Next(0, 10)
                    };
                }
            }
        }
    }
}