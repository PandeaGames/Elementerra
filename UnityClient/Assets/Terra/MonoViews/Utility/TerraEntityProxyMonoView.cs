using System;
using System.Collections.Generic;
using PandeaGames.Data;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using UnityEngine;

namespace Terra.MonoViews.Utility
{
    public class TerraEntityProxyMonoView : MonoBehaviour
    {
        private GameObject _proxy;

        public void Clear()
        {
            Destroy(_proxy);
        }
        
        public void Render(ITerraEntityType terraEntity)
        {
            _proxy =
                Instantiate(TerraGameResources.Instance.TerraEntityPrefabConfig.GetGameObject(terraEntity), transform);
            _proxy.name = $"{terraEntity.EntityID} Proxy";
            
            List<Component> components = new List<Component>();
            _proxy.GetComponents<Component>(components);
            _proxy.GetComponentsInChildren<Component>(components);
            
            foreach (Component comp in components)
            {
                if (comp is Renderer || comp is Transform || comp is MeshFilter || comp is LODGroup)
                {
                    continue;
                }

                try
                {
                    Destroy(comp);
                }
                catch (Exception e)
                {
                    continue;
                }
            }
        }
    }
}