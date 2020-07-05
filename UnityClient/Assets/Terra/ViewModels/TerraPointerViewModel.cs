using System;
using PandeaGames.ViewModels;
using UnityEngine;

namespace Terra.ViewModels
{
    public class TerraPointerViewModel : IViewModel
    {
        public event Action<RaycastHit> OnClick;
        
        public Vector3 MousePosition { get; set; }
        public TerraVector MousePositionTerraVector { get; set; }
        public Vector3 MousePositionOnGrid { get; set; }
        public bool MouseDown { get; set; }
        public RaycastHit Hit { get; private set; }

        public void Reset()
        {
            
        }

        public void Click(RaycastHit hit)
        {
            Hit = hit;
            OnClick?.Invoke(hit);
        }
    }
}