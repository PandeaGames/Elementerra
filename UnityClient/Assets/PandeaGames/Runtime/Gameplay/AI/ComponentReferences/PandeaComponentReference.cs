using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public abstract class PandeaComponentReference<TComponent> : MonoBehaviour where TComponent:Component
    {
        [SerializeField] 
        private TComponent m_componentReference;
        public TComponent Component => m_componentReference;

        public static implicit operator TComponent(PandeaComponentReference<TComponent> componentReference)
        {
            return componentReference.Component;
        }
    }
}