using System;
using UnityEngine;

namespace Terra.Inventory
{
    [Serializable]
    public class InventoryDataType : IInventoryDataType
    {
        [SerializeField]
        private int _maxInventorySize;

        [SerializeField] 
        private string _id;

        [SerializeField]
        private int _maxStackSize;
        
        public int MaxInventorySize
        {
            get => _maxInventorySize;
        }
        public string Id
        {
            get => _id;
        }

        public int MaxStackCount
        {
            get => _maxStackSize;
        }
    }
}