using System;
using System.Collections.Generic;
using Terra.MonoViews.DebugMonoViews;
using UnityEngine;

namespace Terra.MonoViews.Utility
{
    public class TerraEntityColliderMonoView : MonoBehaviour
    {
        public event Action<TerraEntityMonoView> OnEntityTriggerEnter;
        public event Action<TerraEntityMonoView> OnEntityTriggerExit;
        
        private List<TerraEntityMonoView> _collidingWith;
        public IEnumerable<TerraEntityMonoView> CollidingWith => _collidingWith;

        private void Start()
        {
            _collidingWith = new List<TerraEntityMonoView>();
        }

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
                emv.OnViewDestroyed += EmvOnViewDestroyed;
                _collidingWith.Add(emv);
                OnEntityTriggerEnter?.Invoke(emv);
            }
        }

        private void EmvOnViewDestroyed(TerraEntityMonoView view)
        {
            view.OnViewDestroyed -= EmvOnViewDestroyed;
            _collidingWith.Remove(view);
            OnEntityTriggerExit?.Invoke(view);
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
                emv.OnViewDestroyed -= EmvOnViewDestroyed;
                _collidingWith.Remove(emv);
                OnEntityTriggerExit?.Invoke(emv);
            }
        }
    }
}