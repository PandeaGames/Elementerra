using TMPro;
using UnityEngine;

namespace Terra.Inventory.MonoViews
{
    public class InventoryItemMonoView : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI _text;
        
        public void Render(IInventoryItem item)
        {
            _text.text = item.Id;
        }
    }
}