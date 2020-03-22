using Terra.SerializedData.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.MonoViews.DebugMonoViews
{
    public class EntityMonoView : AbstractListItem<TerraEntityTypeData>
    {
        [SerializeField] 
        private Image _image;
        
        public override void SetData(TerraEntityTypeData data)
        {
            if (data.DebugImage != null)
            {
                _image.sprite = data.DebugImage;
            }
            
            base.SetData(data);
        }
    }
}