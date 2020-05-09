using System.Collections;
using System.Collections.Generic;
using PandeaGames;
using PandeaGames.ViewModels;
using PandeaGames.Views;
using Terra.ViewModels;

namespace Terra.Inventory
{
    public delegate void IntentoryItemDelegate(IInventoryItem item);
    
    public class InventoryViewModel : IViewModel, IEnumerable<IInventoryItem>
    {
        public event IntentoryItemDelegate OnAddItem;
        public event IntentoryItemDelegate OnRemoveItem;

        public IInventoryDataType InventoryDataType;
        public int InventoryId;
        private Dictionary<string, IInventoryItem> _stackableItems = new Dictionary<string, IInventoryItem>();
        
        private List<IInventoryItem> _items = new List<IInventoryItem>();
        public IEnumerable<IInventoryItem> Items
        {
            get => _items;
        }

        public void AddItems(IEnumerable<IInventoryItem> items)
        {
            foreach (IInventoryItem item in items)
            {
                AddItem(item);
            }
        }

        public void AddItem(IInventoryItemData inventoryItemData)
        {
            InventoryItemDataSerializable serializable = new InventoryItemDataSerializable();

            serializable.Id = inventoryItemData.Id;
            serializable.InventoryId = InventoryId;
            serializable.TickAdded = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0).State.Tick;
            
            InventoryItem inventoryItem = new InventoryItem(inventoryItemData, serializable);
            AddItem(inventoryItem);
        }
        
        public void AddItem(IInventoryItem item)
        {
            if (!_items.Contains(item))
            {
                _items.Add(item);
                OnAddItem?.Invoke(item);
            }
        }
        
        public bool RemoveItem(IInventoryItem item)
        {
            if (_items.Remove(item))
            {
                OnRemoveItem?.Invoke(item);
                return true;
            }

            return false;
        }

        public void Reset()
        {
            
        }

        public bool IsFull => _items.Count > InventoryDataType.MaxInventorySize;

        public IEnumerator<IInventoryItem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}