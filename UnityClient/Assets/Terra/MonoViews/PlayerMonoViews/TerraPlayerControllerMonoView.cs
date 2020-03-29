using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.MonoViews.Utility;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.Services;
using Terra.ViewModels;
using Terra.WorldContextUI;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraPlayerControllerMonoView : MonoBehaviour
    {
        [SerializeField]
        private TerraEntityColliderMonoView _terraEntityColliderMonoView;
        
        private WorldContextViewModel _contextUIModel;
        private List<TerraEntityMonoView> _collidingWith;
        private TerraEntityMonoView _currentEntityMonoView;
        private PlayerStateViewModel _playerStateViewModel;
        private TerraEntitiesViewModel _terraEntitiesViewModel;
        
        private void Start()
        {
            _collidingWith = new List<TerraEntityMonoView>();
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
            _terraEntityColliderMonoView.OnEntityTriggerEnter += TerraEntityColliderMonoViewOnEntityTriggerEnter;
            _terraEntityColliderMonoView.OnEntityTriggerExit += TerraEntityColliderMonoViewOnEntityTriggerExit;
            _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
        }
        
        private void TerraEntityColliderMonoViewOnEntityTriggerEnter(TerraEntityMonoView obj)
        {
            _collidingWith.Add(obj);
        }

        private void TerraEntityColliderMonoViewOnEntityTriggerExit(TerraEntityMonoView obj)
        {
            _collidingWith.Remove(obj);
        }

        private void Update()
        {
            _currentEntityMonoView = null;
            foreach (TerraEntityMonoView entityMonoView in _collidingWith)
            {
                if (entityMonoView == null || !entityMonoView.Entity.EntityTypeData.Component.HasFlag(EntityComponent.CanPickUp))
                {
                    continue;
                }
                
                if (_currentEntityMonoView == null)
                {
                    _currentEntityMonoView = entityMonoView;
                    continue;
                }

                if (Vector3.Distance(transform.position, entityMonoView.transform.position) <
                    Vector3.Distance(transform.position, _currentEntityMonoView.transform.position))
                {
                    _currentEntityMonoView = entityMonoView;
                }
            }

            if (_currentEntityMonoView)
            {
                _contextUIModel.SetContext(_currentEntityMonoView.transform.position, WorldContextViewModel.Context.PickUp, _currentEntityMonoView.GetHashCode());
            }
            else
            {
                _contextUIModel.ClearContext();
            }

            switch (_contextUIModel.CurrentContext)
            {
                case WorldContextViewModel.Context.PickUp:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!string.IsNullOrEmpty(_playerStateViewModel.State.HoldingEntityID))
                        {
                            CreateHoldingEntity(-1.2f);
                        }
                        
                        _playerStateViewModel.SetHoldingEntityId(_currentEntityMonoView.Entity);
                        _terraEntitiesViewModel.RemoveEntity(_currentEntityMonoView.Entity);
                    }
                    break;
                }
                case WorldContextViewModel.Context.None:
                {
                    if (Input.GetKeyDown(KeyCode.Space) && !string.IsNullOrEmpty(_playerStateViewModel.State.HoldingEntityID))
                    {
                        CreateHoldingEntity(1.2f);
                        _playerStateViewModel.SetHoldingEntityId(string.Empty);
                    }

                    break;
                }
            }
        }

        private void CreateHoldingEntity(float offset)
        {
            TerraEntityTypeData entityType =
                TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(_playerStateViewModel
                    .State.HoldingEntityID);
            RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(entityType);
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);

            if (entityType.Component.HasFlag(EntityComponent.Position))
            {
                entity.Position.Set(
                    new Vector3(
                        transform.position.x + transform.forward.x * offset,
                        transform.position.y + 1,
                        transform.position.z + transform.forward.z * offset));
            }
            
            vm.AddEntity(entity);
            
        }
    }
}