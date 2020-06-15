using System;
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
                this[dataPoint.Vector] = IsPathable(dataPoint, chunk);
            }
        }

        private bool IsPathable(TerraDataPoint dataPoint, TerraWorldChunk chunk)
        {
            var cell = dataPoint.Vector;
            for(int x = Math.Max(0, cell.x - 1); x <= Math.Min(Width - 1, cell.x + 2); x++)
            {
                for(int y = Math.Max(0, cell.y - 1); y <= Math.Min(Height - 1, cell.y + 2); y++)
                {
                    if (x != cell.x && y != cell.y)
                    {
                        if (chunk[x, y].Height < 0)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        
        public TerraPathfinderViewModel(bool[,] data) : base(data)
        {
        }

        public TerraPathfinderViewModel(uint width, uint height) : base(width, height)
        {
        }
    }
}