using System;
using Terra.MonoViews.DebugMonoViews;
using UnityEngine;

namespace Terra.MonoViews.Utility
{
    public class TerraEntityColliderMonoView : MonoBehaviour
    {
        public event Action<TerraEntityMonoView> OnEntityTriggerEnter;
        public event Action<TerraEntityMonoView> OnEntityTriggerExit;
        
        private void OnTriggerEnter(Collider other)
        {
            Transform currentTransform = other.transform;
            TerraEntityMonoView emv = null;

            do
            {
                emv = currentTransform.gameObject.GetComponent<TerraEntityMonoView>();
                currentTransform = currentTransform.parent;
            } while (currentTransform != null && emv == null);

            if (emv != null)
            {
                OnEntityTriggerEnter?.Invoke(emv);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            Transform currentTransform = other.transform;
            TerraEntityMonoView emv = null;

            do
            {
                emv = currentTransform.gameObject.GetComponent<TerraEntityMonoView>();
                currentTransform = currentTransform.parent;
            } while (currentTransform != null && emv == null);
            
            if (emv != null && emv.IsInitialized)
            {
                OnEntityTriggerExit?.Invoke(emv);
            }
        }
    }
}