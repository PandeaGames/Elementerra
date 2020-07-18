using PandeaGames;
using Terra.ViewModels;
using UnityEditor.UIElements;
using UnityEngine;

namespace Terra.MonoViews
{
    [RequireComponent(typeof(Camera))]
    public class TerraCameraMonoView : MonoBehaviour
    {
        private int _alphaMask;
        private int _betaMask;
        private Camera _camera;
        private TerraWorldStateViewModel _worldStateViewModel;

        [SerializeField]
        private LayerMask _alphaLayerMask;
        
        [SerializeField]
        private LayerMask _betaLayerMask;
        
        private void Start()
        {
            _alphaMask = LayerMask.NameToLayer("AlphaDimension");
            _betaMask = LayerMask.NameToLayer("BetaDimension");
            _camera = GetComponent<Camera>();
            _worldStateViewModel = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _worldStateViewModel.OnWorldFlipChange += UpdateState;
            UpdateState(_worldStateViewModel.IsWorldFipped);
        }

        private void OnDestroy()
        {
            _worldStateViewModel.OnWorldFlipChange -= UpdateState;
        }

        private void UpdateState(bool state)
        {
            _camera.cullingMask = state ? _betaLayerMask : _alphaLayerMask;
            /*var value = MyEnum.Flag1;

// set additional value
value |= MyEnum.Flag2;  //value is now Flag1, Flag2
value |= MyEnum.Flag3;  //value is now Flag1, Flag2, Flag3

// remove flag
value &= ~MyEnum.Flag2; //value is now Flag1, Flag3 */
            // _camera.cullingMask.
        }
    }
}