namespace PandeaGames.ViewModels
{
    public class BoolGridNode : GridDataPoint<bool>
    {
        
    } 
    
    public class PathfinderViewModel : AbstractGridDataModel<bool, BoolGridNode>
    {
        public PathfinderViewModel(bool[,] data) : base(data)
        {
        }

        public PathfinderViewModel(uint width, uint height) : base(width, height)
        {
        }
    }
}