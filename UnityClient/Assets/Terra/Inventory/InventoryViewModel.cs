using System.Collections;
using System.Collections.Generic;
using PandeaGames.ViewModels;
using PandeaGames.Views;

namespace Terra.Inventory
{
    public delegate void IntentoryItemDelegate(IInventoryItem item);
    
    public class InventoryViewModel : IViewModel, IEnumerable<IInventoryItem>
    {
        public event IntentoryItemDelegate OnAddItem;
        public event IntentoryItemDelegate OnRemoveItem;

        public IInventoryDataType InventoryDataType;
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