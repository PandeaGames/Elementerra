using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.Inventory.MonoViews
{
    public class InventoryMonoView : MonoBehaviour
    {
        [SerializeField] 
        private InventoryItemMonoView _inventoryItemMonoViewPrefab;
        
        [SerializeField] 
        private Transform _itemContainer;

        private Dictionary<IInventoryItem, InventoryItemMonoView> _views;
        
        public void Render(InventoryViewModel inventory)
        {
            if (_views != null)
            {
                foreach (KeyValuePair<IInventoryItem, InventoryItemMonoView> kvp in _views)
                {
                    Destroy(kvp.Value.gameObject);
                }
            }
            
            _views = new Dictionary<IInventoryItem, InventoryItemMonoView>();
            inventory.OnAddItem += InventoryOnAddItem;
            inventory.OnRemoveItem -= InventoryOnRemoveItem;

            foreach (IInventoryItem item in inventory)
            {
                SetupInventoryItem(item);
            }
        }

        private void InventoryOnRemoveItem(IInventoryItem item)
        {
            if (_views.TryGetValue(item, out InventoryItemMonoView inventoryItemMonoView))
            {
                Destroy(inventoryItemMonoView.gameObject);
                _views.Remove(item);
            }
        }

        private void InventoryOnAddItem(IInventoryItem item)
        {
            SetupInventoryItem(item);
        }

        private void SetupInventoryItem(IInventoryItem inventoryItem)
        {
            InventoryItemMonoView inventoryItemMonoView = Instantiate(_inventoryItemMonoViewPrefab.gameObject, _itemContainer,
                worldPositionStays: false).GetComponent<InventoryItemMonoView>();
            inventoryItemMonoView.gameObject.SetActive(true);
            inventoryItemMonoView.Render(inventoryItem);
            _views.Add(inventoryItem, inventoryItemMonoView);
        }
    }
}