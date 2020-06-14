using PandeaGames.ViewModels;
using Terra.SerializedData.World;

namespace Terra.ViewModels
{
    public class TerraPathfinderViewModel : PathfinderViewModel
    {
        public TerraPathfinderViewModel(TerraWorldChunk chunk) : base(new bool[chunk.Width,chunk.Height])
        {
            BuildData(chunk);
        }

        private void BuildData(TerraWorldChunk chunk)
        {
            foreach (TerraDataPoint dataPoint in chunk.AllData())
            {
                this[dataPoint.Vector] = dataPoint.Data.Height >= 0;
            }
        }
        
        public TerraPathfinderViewModel(bool[,] data) : base(data)
        {
        }

        public TerraPathfinderViewModel(uint width, uint height) : base(width, height)
        {
        }
    }
}