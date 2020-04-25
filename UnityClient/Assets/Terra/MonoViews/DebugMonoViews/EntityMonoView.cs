using System;
using PandeaGames.Utils;
using Terra.MonoViews.Utility;
using Terra.SerializedData.GameData;
using UnityEngine;
using UnityEngine.UI;

namespace Terra.MonoViews.DebugMonoViews
{
    public class EntityMonoView : AbstractListItem<TerraEntityTypeData>
    {
        [SerializeField] 
        private Image _image;
        
        [SerializeField] 
        private RawImage _rawImage;
        
        public override void SetData(TerraEntityTypeData data)
        {
            if (data.DebugImage != null)
            {
                _image.sprite = data.DebugImage;
            }

           // GameObject proxy = CreateProxyEntity(data);
            //LightboxManager.GetTexture(proxy, texture2D => { _rawImage.texture = texture2D; }, () => { });
            
            base.SetData(data);
        }
        
        private GameObject CreateProxyEntity(TerraEntityTypeData entityData)
        {
            TerraEntityProxyMonoView instance = new GameObject("Proxy", new Type[]{typeof(TerraEntityProxyMonoView)}).GetComponent<TerraEntityProxyMonoView>();
            instance.Render(entityData);
            return instance.gameObject;
        }
    }
}