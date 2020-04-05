using System;
using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Data;
using PandeaGames.Services;
using Terra.SerializedData.GameData;
using Terra.Services;
using Terra.ViewModels;

namespace Terra.Inventory
{
    public class InventoryService : IService
    {
        private Dictionary<int, InventoryViewModel> _models = new Dictionary<int, InventoryViewModel>();
        
        public void GetInventory(int inventoryId, IInventoryDataType type, Action<InventoryViewModel> onComplete, Action onError)
        {
            InventoryViewModel model = null;

            if (!_models.TryGetValue(inventoryId, out model))
            {
                InventoryItemDataSerializable[] items = Game.Instance.GetService<TerraDBService>().Get<InventoryItemDataSerializer, InventoryItemDataSerializable>(
                    InventoryItemDataSerializer.Instance,
                    "",
                    $"SELECT rowid, * FROM {InventoryItemDataSerializer.Instance.Table} WHERE inventoryId = '{inventoryId}'"
                );

                model = Game.Instance.GetViewModel<InventoryViewModel>((uint)inventoryId);
                model.InventoryDataType = type;

                foreach (InventoryItemDataSerializable serializableItem in items)
                {
                    if (TerraGameResources.Instance.InventoryItemDataListSO.TryGetInventoryItemData(serializableItem.Id,
                        out IInventoryItemData itemData))
                    {
                        InventoryItem item = new InventoryItem(
                            itemData,
                            serializableItem);
                        model.AddItem(item);
                    }
                }
                
                _models.Add(inventoryId, model);
                model.OnAddItem += ModelOnAddItem;
                model.OnRemoveItem += ModelOnRemoveItem;
            }
            
            onComplete?.Invoke(model);
        }

        private void ModelOnRemoveItem(IInventoryItem item)
        {
            Game.Instance.GetService<TerraDBService>().DeleteRow(item.Serializable.RowId, InventoryItemDataSerializer.Instance);
        }

        private void ModelOnAddItem(IInventoryItem item)
        {
            Game.Instance.GetService<TerraDBService>().WriteNewRecord(item.Serializable, InventoryItemDataSerializer.Instance);
        }

        public void AddItem(int inventoryId, IInventoryItemData inventoryItemData)
        {
            InventoryItemDataSerializable serializable = new InventoryItemDataSerializable();

            serializable.Id = inventoryItemData.Id;
            serializable.InventoryId = inventoryId;
            serializable.TickAdded = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0).State.Tick;
            
            InventoryItem inventoryItem = new InventoryItem(inventoryItemData, serializable);
            _models[inventoryId].AddItem(inventoryItem);
        }
    }
}