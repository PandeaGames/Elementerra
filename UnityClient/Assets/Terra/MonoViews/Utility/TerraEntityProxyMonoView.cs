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
        private ITerraEntityType _currentRenderingProxy;

        public void Clear()
        {
            Destroy(_proxy);
        }

        public void Render(string entityType)
        {
            if (string.IsNullOrEmpty(entityType))
            {
                if (_proxy != null)
                {
                    Destroy(_proxy);
                }
            }
            else
            {
                ITerraEntityType terraEntity =
                    TerraGameResources.Instance.TerraEntityPrefabConfig.GetEntityConfig(entityType);
                Render(terraEntity);
                _currentRenderingProxy = terraEntity;
            }
        }

        public void Render(ITerraEntityType terraEntity)
        {
            if (
                terraEntity == null ||
                _currentRenderingProxy != null && 
                terraEntity.EntityID == _currentRenderingProxy.EntityID)
            {
                return;
            }

            if (_proxy != null)
            {
                Clear();
            }
            
            _proxy =
                Instantiate(TerraGameResources.Instance.TerraEntityPrefabConfig.GetGameObject(terraEntity), transform);
            _proxy.name = $"{terraEntity.EntityID} Proxy";
            
            List<Component> components = new List<Component>();
            _proxy.GetComponents<Component>(components);
            _proxy.GetComponentsInChildren<Component>(components);
            
            foreach (Component comp in components)
            {
                if (comp is Renderer || comp is Transform || comp is MeshFilter || comp is LODGroup || comp is Light)
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