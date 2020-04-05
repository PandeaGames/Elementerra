using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.Inventory;
using Terra.Inventory.MonoViews;
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
                if (entityMonoView == null || 
                    (!entityMonoView.Entity.EntityTypeData.Component.HasFlag(EntityComponent.CanPickUp) && 
                     entityMonoView.Entity.EntityTypeData.InventoryItemDataSO == null &&
                     !(entityMonoView.Entity.EntityTypeData.Component.HasFlag(EntityComponent.Harvestable) 
                       && entityMonoView.Entity.IsRipe())))
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
                if (_currentEntityMonoView.Entity.EntityTypeData.InventoryItemDataSO != null)
                {
                    _contextUIModel.SetContext(_currentEntityMonoView.transform.position, WorldContextViewModel.Context.PutInInventory, _currentEntityMonoView.Entity);
                }
                else if (!_playerStateViewModel.IsHoldingItem)
                {
                    _contextUIModel.SetContext(_currentEntityMonoView.transform.position, WorldContextViewModel.Context.PickUp, _currentEntityMonoView.Entity);
                }
            }
            else if(!_playerStateViewModel.IsHoldingItem)
            {
                _contextUIModel.ClearContext();
            }
            else if(_playerStateViewModel.IsHoldingItem)
            {
                _contextUIModel.SetContext(_contextUIModel.CurrentTransform, WorldContextViewModel.Context.Holding, _playerStateViewModel);
            }

            switch (_contextUIModel.CurrentContext)
            {
                case WorldContextViewModel.Context.PutInInventory:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        AddToInventory();
                        
                    }

                    break;
                }
                case WorldContextViewModel.Context.PickUp:
                {
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (_currentEntityMonoView.Entity.IsRipe())
                        {
                            HarvestEntity(1.2f);
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(_playerStateViewModel.State.HoldingEntityID))
                            {
                                CreateHoldingEntity(-1.2f);
                            }
                        
                            _playerStateViewModel.SetHoldingEntityId(_currentEntityMonoView.Entity);
                            _terraEntitiesViewModel.RemoveEntity(_currentEntityMonoView.Entity);
                            _contextUIModel.SetContext(_currentEntityMonoView.transform.position, WorldContextViewModel.Context.Holding, _playerStateViewModel);
                        }
                    }
                    break;
                }
                case WorldContextViewModel.Context.None:
                case WorldContextViewModel.Context.Holding:
                {
                    if (!string.IsNullOrEmpty(_playerStateViewModel.State.HoldingEntityID))
                    {
                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            CreateHoldingEntity(1.2f);
                            _playerStateViewModel.SetHoldingEntityId(string.Empty);
                        }
                        else  if (Input.GetKeyDown(KeyCode.E))
                        {
                            PlantHoldingEntity();
                            _playerStateViewModel.SetHoldingEntityId(string.Empty);
                        }
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

        private void AddToInventory()
        {
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            vm.RemoveEntity(_currentEntityMonoView.Entity);
            Game.Instance.GetService<InventoryService>().AddItem(TerraGameResources.PLAYER_INSTANCE_ID,_currentEntityMonoView.Entity.EntityTypeData.InventoryItemDataSO.Data );
        }

        private void HarvestEntity(float offset)
        {
            TerraEntityTypeData spawnableEntityType =
                TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(_currentEntityMonoView.Entity.EntityTypeData.SpawnableEntityId);
            
            RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(spawnableEntityType);
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            
            entity.Position.Set(
                new Vector3(
                    transform.position.x + transform.forward.x + offset,
                    transform.position.y + 1,
                    transform.position.z + transform.forward.z + offset));
            
            vm.AddEntity(entity);
            vm.RemoveEntity(_currentEntityMonoView.Entity);            
        }

        private void PlantHoldingEntity()
        {
            TerraEntityTypeData entityType =
                TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(_playerStateViewModel
                    .State.HoldingEntityID);
            
            TerraEntityTypeData plantEntity =
                TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(entityType.PlantableEntityId);
            RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(plantEntity);
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            TerraViewModel gmViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            
            entity.GridPosition.Set(new TerraVector((int)gmViewModel.PlayerPosition.x, (int)gmViewModel.PlayerPosition.z));
            
            vm.AddEntity(entity);
        }
    }
}