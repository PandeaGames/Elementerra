using System;
using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.GameData;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraDebugPlaceEntityMonoView : TerraDebugStatefulMonoView
    {
        [SerializeField] 
        private LayerMask _layerMaskForMousePosition;
        
        private Vector3 _mousePosition;
        public Vector3 MousePosition => _mousePosition;
        private TerraVector _mousePositionOnTerra;
        public TerraVector MousePositionOnTerra => _mousePositionOnTerra;
        private Vector2 _mousePositionOnGrid;
        public Vector2 MousePositionOnGrid => _mousePositionOnGrid;
        private Vector3 _mousePosition3OnGrid;
        public Vector3 MousePosition3OnGrid => _mousePosition3OnGrid;
        
        private GameObject _entityProxy;
        private TerraDebugControlViewModel _vm;

        private void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
            _vm.OnEnterState += OnEnterState;
        }

        private void OnEnterState(TerraDebugControlViewModel.States state)
        {
            switch (state)
            {
                case TerraDebugControlViewModel.States.None:
                {
                    if (_entityProxy != null)
                    {
                        Destroy(_entityProxy);
                    }
                    break;
                }
                case TerraDebugControlViewModel.States.PlaceEntity:
                {
                    if (_entityProxy != null)
                    {
                        Destroy(_entityProxy);
                    }
                    
                    _entityProxy = CreateProxyEntity(_vm.entityData);
                    break;
                }
                case TerraDebugControlViewModel.States.Sculpt:
                {
                    break;
                }
            }
        }

        private GameObject CreateProxyEntity(TerraEntityTypeData entityData)
        {
            GameObject instance =
                Instantiate(TerraGameResources.Instance.TerraEntityPrefabConfig.GetGameObject(entityData), transform);
            instance.name = $"{entityData.EntityID} Proxy";
            foreach (Component comp in instance.GetComponents<Component>())
            {
                if (comp is Renderer || comp is Transform)
                {
                    continue;
                }

                try
                {
                    Destroy(comp);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            
            return instance;
        }

        private void Update()
        {
            if (_currentState == TerraDebugWindowMonoView.EditorStates.Locked)
            {
                if (_vm.CurrentState == TerraDebugControlViewModel.States.PlaceEntity && _entityProxy != null)
                {
                    _entityProxy.SetActive(true);
                    UpdateProxy(_entityProxy);
                }
            }
            else if(_entityProxy != null)
            {
                _entityProxy.SetActive(false);
            }
        }

        private void UpdateProxy(GameObject proxy)
        {
            Ray ray = _vm.DebugCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, _layerMaskForMousePosition,
                QueryTriggerInteraction.Collide))
            {
                _mousePosition = hit.point;
                _mousePositionOnGrid = new Vector2(MathUtils.Round(_mousePosition.x, 1),
                    MathUtils.Round(_mousePosition.z, 1));
                int x = (int) Math.Round(_mousePositionOnGrid.x);
                int y = (int) Math.Round(_mousePositionOnGrid.y);
                _mousePositionOnTerra = new TerraVector() {x = x, y = y};
                /*TerraVector mousePositionOnToken = new TerraVector()
                    {x = x - _currentToken.Request.left, y = y - _currentToken.Request.top};
                _mousePosition3OnGrid = GetVertice(_currentToken, mousePositionOnToken.x, mousePositionOnToken.y);*/
                // Do something with the object that was hit by the raycast.
            }
                
            proxy.transform.position = _mousePosition;
        }
    }
}