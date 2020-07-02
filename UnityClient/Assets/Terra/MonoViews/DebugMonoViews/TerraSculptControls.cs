using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraSculptControls : MonoBehaviour
    {
        [SerializeField]
        private Projector _projector;
        
        [SerializeField]
        private Light _light;

        [SerializeField]
        private Material _material;
        
        private TerraSculptViewModel _terraSculptViewModel;
        private TerraDebugControlViewModel _terraDebugControlViewModel;
        private TerraPointerViewModel _terraPointerViewModel;
        private void Start()
        {
            _terraPointerViewModel = Game.Instance.GetViewModel<TerraPointerViewModel>(0);
            _terraSculptViewModel = Game.Instance.GetViewModel<TerraSculptViewModel>(0);
            _terraDebugControlViewModel = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
        }

        private void Update()
        {
            _projector.material = _material;
            _projector.orthographic = true;
            _projector.orthographicSize = _terraSculptViewModel.Size;
            _projector.transform.position = _terraPointerViewModel.MousePosition + new Vector3(0, _terraSculptViewModel.Size, 0);
            _projector.transform.rotation = Quaternion.Euler(90, 0, 0);
            _light.spotAngle = 45;
            _light.intensity = _terraSculptViewModel.Size * 3;
        }
    }
}