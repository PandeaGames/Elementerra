using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public abstract class AbstractPandeaLocomotion : MonoBehaviour
    {
        [SerializeField] 
        protected Transform m_transform;
        
        public abstract void SetRotation(Quaternion rotation);
        public abstract void LootAt(Transform transform);
        public abstract void Move(Quaternion direction, uint moveValue);
        public abstract void Move(Vector3 direction, uint moveValue);
    }
}