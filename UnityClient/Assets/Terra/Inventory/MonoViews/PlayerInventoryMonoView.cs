using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.GameData;
using Terra.SerializedData.GameState;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.Inventory.MonoViews
{
    public class PlayerInventoryMonoView : MonoBehaviour
    {
        public InventoryMonoView _inventoryMonoView;
        public InventoryMonoView _holdingInventoryMonoView;
        private InventoryService _inventoryService;
        private PlayerStateViewModel _playerStateViewModel;

        private void Start()
        {
            _inventoryService = Game.Instance.GetService<InventoryService>();
            TerraGameResources resources = TerraGameResources.Instance;
            _inventoryService.GetInventory(
                TerraGameResources.PLAYER_INSTANCE_ID,
                resources.PlayerInventoryType.Data,
                OnInventoryLoaded,
                null
            );
            

            _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _playerStateViewModel.OnChange += PlayerStateViewModelOnChange;
            PlayerStateViewModelOnChange(_playerStateViewModel.State);
        }

        private void PlayerStateViewModelOnChange(TerraPlayerState state)
        {
            _holdingInventoryMonoView.gameObject.SetActive(false);
            if (_playerStateViewModel.IsHoldingItem)
            {
                TerraEntityTypeData entityData =
                    TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(state.HoldingEntityID);
                _holdingInventoryMonoView.gameObject.SetActive(entityData.Inventory != null);
                if (entityData.Inventory != null)
                {
                    _inventoryService.GetInventory(
                        state.HoldingInstanceID,
                        entityData.Inventory,
                        inventory => _holdingInventoryMonoView.Render(inventory),
                        null
                    );
                }
            }
        }

        private void OnInventoryLoaded(InventoryViewModel inventory)
        {
            _inventoryMonoView.Render(inventory);
        }
    }
}