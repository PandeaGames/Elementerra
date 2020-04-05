using System;
using UnityEngine;

namespace Terra.Inventory
{
    [Serializable]
    public class InventoryItemData : IInventoryItemData
    {
        [SerializeField] private bool _isStackable;
        [SerializeField] private string _id;
        
        public bool IsStackable
        {
            get => _isStackable;
        }
        public string Id
        {
            get => _id;
        }
    }
}