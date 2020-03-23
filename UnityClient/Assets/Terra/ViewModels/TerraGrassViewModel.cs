using System;

namespace Terra.ViewModels
{
    public class TerraGrassNodeGridPoint : GridDataPoint<TerraGrassNode>
    {
        
    }
    
    public struct TerraGrassNode
    {
        public int Grass;
    }
    
    public class TerraGrassViewModel : AbstractGridDataModel<TerraGrassNode, TerraGrassNodeGridPoint>
    {
        public TerraGrassViewModel(TerraTerrainGeometryDataModel terrainModel) : base(new TerraGrassNode[terrainModel.Height,terrainModel.Width])
        {
            Random rand = new Random();
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Width; y++)
                {
                    this[x, y] = new TerraGrassNode()
                    {
                        Grass = rand.Next(0, 10)
                    };
                }
            }
        }
    }
}