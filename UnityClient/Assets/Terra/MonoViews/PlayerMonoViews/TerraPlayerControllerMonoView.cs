using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using Terra.Inventory;
using Terra.Inventory.MonoViews;
using Terra.MonoViews.Utility;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.SerializedData.GameState;
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
        private PlayerEntitySlaveViewModel _playerEntitySlaveViewModel;
        private InventoryService _inventoryService;

        private InventoryViewModel _playerInventoryViewModel;
        private InventoryViewModel _holdingInventoryViewModel;

        private void Start()
        {
            _playerEntitySlaveViewModel = Game.Instance.GetViewModel<PlayerEntitySlaveViewModel>(0);
            _collidingWith = new List<TerraEntityMonoView>();
            _contextUIModel = Game.Instance.GetViewModel<WorldContextViewModel>(0);
            _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            
            _inventoryService = Game.Instance.GetService<InventoryService>();
            
            _inventoryService.GetInventory(
                TerraGameResources.PLAYER_INSTANCE_ID,
                TerraGameResources.Instance.PlayerInventoryType.Data,
                playerInventory => _playerInventoryViewModel = playerInventory,
                null
            );

            _playerStateViewModel.OnChange += UpdateHoldingItem;
            UpdateHoldingItem(_playerStateViewModel.State);
        }

        private void UpdateHoldingItem(TerraPlayerState state)
        {
            if (_playerStateViewModel.IsHoldingItem)
            {
                TerraEntityTypeData entityData =
                    TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(state.HoldingEntityID);

                if (entityData.Inventory != null)
                {
                    _inventoryService.GetInventory(
                        state.HoldingInstanceID,
                        entityData.Inventory,
                        inventory => _holdingInventoryViewModel = inventory,
                        null
                    );
                }
            }
        }

        private void Update()
        {
            _currentEntityMonoView = null;
            foreach (TerraEntityMonoView entityMonoView in _terraEntityColliderMonoView.CollidingWith)
            {
                RuntimeTerraEntity entity = entityMonoView.Entity;
                if (entityMonoView == null || 
                    (entity.IsSlavable && _playerEntitySlaveViewModel.CurrentSlave == entity) ||
                    (!entity.EntityTypeData.Component.HasFlag(EntityComponent.CanPickUp) && 
                     entity.EntityTypeData.InventoryItemDataSO == null &&
                     !entity.IsSlavable &&
                     !(entity.EntityTypeData.Component.HasFlag(EntityComponent.Harvestable)
                       && entity.IsRipe())))
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
                if (_currentEntityMonoView.Entity.IsSlavable)
                {
                    _contextUIModel.SetContext(_currentEntityMonoView.transform.position, WorldContextViewModel.Context.Enslave, _currentEntityMonoView.Entity);
                }
                else if (_currentEntityMonoView.Entity.EntityTypeData.InventoryItemDataSO != null)
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
                case WorldContextViewModel.Context.Enslave:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        Game.Instance.GetViewModel<PlayerEntitySlaveViewModel>(0).SetSlave(_currentEntityMonoView.Entity);
                    }
                    
                    break;
                }
                case WorldContextViewModel.Context.PutInInventory:
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        AddToInventory();
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        HoldInHand();
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
                            _playerStateViewModel.ClearHoldingEntityId();
                        }
                        else  if (Input.GetKeyDown(KeyCode.E))
                        {
                            PlantHoldingEntity();
                            _playerStateViewModel.ClearHoldingEntityId();
                        }
                    }
                    
                    if (_playerStateViewModel.IsHoldingItemInHand)
                    {
                        if (Input.GetKeyDown(KeyCode.Q))
                        {
                            PlantHoldingEntity();
                            _playerStateViewModel.ClearHoldingEntityId();
                        }
                    }

                    break;
                }
            }
        }

        private void OnDestroy()
        {
            if (_playerStateViewModel != null)
            {
                _playerStateViewModel.OnChange -= UpdateHoldingItem;
            }
        }

        private void AddToInventory()
        {
            bool wasAddedToInventory = false;
            if (_playerInventoryViewModel.IsFull)
            {
                if (_holdingInventoryViewModel != null && !_holdingInventoryViewModel.IsFull)
                {
                    wasAddedToInventory = true;
                    _holdingInventoryViewModel.AddItem(_currentEntityMonoView.Entity.EntityTypeData.InventoryItemDataSO.Data);
                }
            }
            else
            {
                wasAddedToInventory = true;
                _playerInventoryViewModel.AddItem(_currentEntityMonoView.Entity.EntityTypeData.InventoryItemDataSO.Data);
            }

            if (wasAddedToInventory)
            {
                TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
                vm.RemoveEntity(_currentEntityMonoView.Entity);
            }
        }
        
        private void HoldInHand()
        {
            PlayerStateViewModel playerViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            
            if (playerViewModel.IsHoldingItemInHand)
            {
                CreateEntity(1.2f, playerViewModel.State.HoldingInHandEntityId, playerViewModel.State.HoldingInHandEntityInstanceId);
            }
            
            TerraEntitiesViewModel vm = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            vm.RemoveEntity(_currentEntityMonoView.Entity);
            Game.Instance.GetViewModel<PlayerStateViewModel>(0).SetHoldingInHandEntityId(_currentEntityMonoView.Entity);
        }

        private void DropItemInHand()
        {
            PlayerStateViewModel playerViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            CreateEntity(1.2f, playerViewModel.State.HoldingInHandEntityId, playerViewModel.State.HoldingInHandEntityInstanceId);
            playerViewModel.ClearHoldingInHandEntityId();
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
            TerraPosition3DComponent positionComponent = gmViewModel.PlayerEntity.Entity.Position;
            entity.GridPosition.Set(new TerraVector((int)positionComponent.Data.x, (int)positionComponent.Data.z));
            
            vm.AddEntity(entity);
        }

        private void CreateHoldingEntity(float offset)
        {
            CreateEntity(
                offset, 
                _playerStateViewModel.State.HoldingEntityID, 
                _playerStateViewModel.State.HoldingInstanceID);
        }
        
        private void CreateEntity(float offset, string entityTypeString, int instanceId)
        {
            TerraEntityTypeData entityType =
                TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(entityTypeString);
            RuntimeTerraEntity entity = Game.Instance.GetService<TerraEntitesService>().CreateEntity(entityType);
            entity.InstanceId = instanceId;
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