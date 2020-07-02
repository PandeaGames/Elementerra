using PandeaGames.ViewModels;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraPointerViewModel : IViewModel
    {
        public Vector3 MousePosition { get; set; }
        public TerraVector MousePositionTerraVector { get; set; }
        public Vector3 MousePositionOnGrid { get; set; }

        public void Reset()
        {
            
        }
    }
}