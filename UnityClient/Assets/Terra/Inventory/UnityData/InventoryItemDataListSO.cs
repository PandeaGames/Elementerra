using System.Collections.Generic;
using PandeaGames.Data.Static;
using UnityEngine;

namespace Terra.Inventory.UnityData
{
    [CreateAssetMenu(menuName = "Terra/Inventory/InventoryItemDataListSO")]
    public class InventoryItemDataListSO : AbstractDataContainerSO<List<InventoryItemDataSO>>
    {
        public bool TryGetInventoryItemData(string id, out IInventoryItemData data)
        {
            data = null;
            foreach (InventoryItemDataSO InventoryItemDataSO in Data)
            {
                if (InventoryItemDataSO.Data.Id == id)
                {
                    data = InventoryItemDataSO.Data;
                    return true;
                }
            }
            
            return false;
        }
    }
}