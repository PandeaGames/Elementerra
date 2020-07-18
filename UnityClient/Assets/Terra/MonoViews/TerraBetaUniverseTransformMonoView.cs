using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraBetaUniverseTransformMonoView : MonoBehaviour
    {
        [SerializeField] 
        private Vector3 _offset;
        
        private TerraWorldStateViewModel _worldStateViewModel;
        
        private void Start()
        {
            _worldStateViewModel =  Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            UpdateDimension(_worldStateViewModel.IsWorldFipped);
            _worldStateViewModel.OnWorldFlipChange += UpdateDimension;
        }

        private void OnDestroy()
        {
            _worldStateViewModel.OnWorldFlipChange -= UpdateDimension;
        }

        private void UpdateDimension(bool isWorldFlipped)
        {
            Vector3 offset = isWorldFlipped ? _offset : _offset * -1;
            transform.position = transform.position + offset;
        }
    }
}