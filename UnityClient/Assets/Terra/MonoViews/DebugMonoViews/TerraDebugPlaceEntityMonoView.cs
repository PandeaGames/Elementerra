using System;
using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraDebugPlaceEntityMonoView : TerraDebugStatefulMonoView
    {
        [SerializeField] 
        private LayerMask _layerMaskForMousePosition;
        
        [SerializeField] 
        private LayerMask _generalClickMask;
        
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
            
            List<Component> components = new List<Component>();
            instance.GetComponents<Component>(components);
            instance.GetComponentsInChildren<Component>(components);
            
            foreach (Component comp in components)
            {
                if (comp is Renderer || comp is Transform || comp is MeshFilter || comp is LODGroup)
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
            switch (_currentState)
            {
                case TerraDebugWindowMonoView.EditorStates.Locked:
                {
                    switch (_vm.CurrentState)
                    {
                        case TerraDebugControlViewModel.States.PlaceEntity:
                        {
                            if (_entityProxy != null)
                            {
                                _entityProxy.SetActive(true);
                                UpdateProxy(_entityProxy);
                                if (Input.GetMouseButtonDown(0))
                                {
                                    AddEntity();
                                } 
                            }
                            
                            break;
                        }
                        case TerraDebugControlViewModel.States.None:
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                ProcessGeneralClick();
                            }

                            break;
                        }
                        case TerraDebugControlViewModel.States.MoveEntity:
                        {
                            if (Input.GetMouseButtonUp(0))
                            {
                                _vm.SetState(TerraDebugControlViewModel.States.None);
                            }
                            else
                            {
                                ProcessMovingEntity();
                            }

                            break;
                        }
                    }
                    break;
                }
                case TerraDebugWindowMonoView.EditorStates.FreeFly:
                case TerraDebugWindowMonoView.EditorStates.Off:
                {
                    if (_entityProxy != null)
                    {
                        _entityProxy.SetActive(false);
                    }
                    break;
                }
            }
        }

        private void ProcessGeneralClick()
        {
            Ray ray = _vm.DebugCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, _generalClickMask,
                QueryTriggerInteraction.Collide))
            {
                TerraSerializedEntityPositionMonoView positionMonoView = hit.transform.GetComponent<TerraSerializedEntityPositionMonoView>();

                if (positionMonoView)
                {
                    _vm.MoveEntity(positionMonoView);
                }
            }
        }

        private void ProcessMovingEntity()
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
                
            _vm.movingEntity.transform.position = new Vector3(_mousePosition.x, _mousePosition.y + 1, _mousePosition.z);
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
                
            proxy.transform.position = new Vector3(_mousePosition.x, _mousePosition.y + 1, _mousePosition.z);
        }

        private void AddEntity()
        {
            RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(_vm.entityData);
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);

            entity.Position.Set(
                new Vector3(
                    _entityProxy.transform.position.x,
                    _entityProxy.transform.position.y,
                    _entityProxy.transform.position.z));
            

            vm.AddEntity(entity);
        }
    }
}