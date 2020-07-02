using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.Utility
{
    public class TerraPointerDataMonoView : MonoBehaviour
    {
        [SerializeField] 
        private LayerMask _layerMaskForMousePosition;

        [SerializeField] 
        private uint _viewModelInstance;

        [SerializeField] 
        private string _cameraTag;

        private TerraPointerViewModel _vm;
        private TerraViewModel _terraViewModel;

        private void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraPointerViewModel>(_viewModelInstance);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
        }

        private void Update()
        {
            GameObject go = GameObject.FindGameObjectWithTag(_cameraTag);

            if (go == null)
            {
                Debug.LogWarning($"Cannot find object with tag {_cameraTag}");
                return;
            }

            Camera camera = go.GetComponent<Camera>();
            
            if (camera == null)
            {
                Debug.LogWarning($"Cannot find camera with tag {_cameraTag}");
                return;
            }
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, _layerMaskForMousePosition,
                QueryTriggerInteraction.Collide))
            {
                _vm.MousePosition = hit.point;
                int x = (int) Math.Round(_vm.MousePosition.x);
                int y = (int) Math.Round(_vm.MousePosition.z);
                _vm.MousePositionTerraVector = new TerraVector() {x = x, y = y};
                _vm.MousePositionOnGrid = _terraViewModel.Geometry.TryGetClosestGridPosition(_vm.MousePosition);
            }
        }
    }
}