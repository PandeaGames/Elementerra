using PandeaGames;
using PandeaGames.Data;
using UnityEngine;

namespace Terra.Inventory.MonoViews
{
    public class PlayerInventoryMonoView : MonoBehaviour
    {
        public InventoryMonoView _inventoryMonoView;

        private void Start()
        {
            TerraGameResources resources = TerraGameResources.Instance;
            Game.Instance.GetService<InventoryService>().GetInventory(
                TerraGameResources.PLAYER_INSTANCE_ID,
                resources.PlayerInventoryType.Data,
                OnInventoryLoaded,
                null
            );
        }

        private void OnInventoryLoaded(InventoryViewModel inventory)
        {
            _inventoryMonoView.Render(inventory);
        }
    }
}