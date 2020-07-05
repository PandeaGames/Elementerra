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
        private LayerMask _generalClickMask;

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

        private bool FindCamera(out Camera camera)
        {
            camera = null;
            GameObject go = GameObject.FindGameObjectWithTag(_cameraTag);

            if (go == null)
            {
                Debug.LogWarning($"Cannot find object with tag {_cameraTag}");
                return false;
            }

            camera = go.GetComponent<Camera>();
            
            if (camera == null)
            {
                Debug.LogWarning($"Cannot find camera with tag {_cameraTag}");
                return false;
            }
            
            return true;
        }

        private void Update()
        {
            if (FindCamera(out Camera camera))
            {
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
            
            if (Input.GetMouseButtonDown(0) && !_vm.MouseDown)
            {
                _vm.MouseDown = true;
                ProcessGeneralClick();
            } 
            else if (Input.GetMouseButtonUp(0) && _vm.MouseDown)
            {
                _vm.MouseDown = false;
            }
        }
        
        private void ProcessGeneralClick()
        {
            if (FindCamera(out Camera camera))
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, _generalClickMask,
                    QueryTriggerInteraction.Collide))
                {
                    _vm.Click(hit);
                }
            }
        }
    }
}