using System.Collections.Generic;
using PandeaGames.Data.Static;
using UnityEngine;

namespace Terra.Inventory.UnityData
{
    [CreateAssetMenu(menuName = "Terra/Inventory/InventoryTypes")]
    public class InventoryTypesSO : AbstractDataContainerSO<List<InventoryDataTypeSO>>
    {
        public IInventoryDataType GetType(string id)
        {
            foreach (InventoryDataTypeSO InventoryDataType in Data)
            {
                if (InventoryDataType.Data.Id == id)
                {
                    return InventoryDataType.Data;
                }
            }
            
            return null;
        }
    }
}