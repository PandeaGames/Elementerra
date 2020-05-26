using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaPlayAnimationState : AbstractPandeaState
    {
        [SerializeField] 
        private AnimatorReference m_animator;

        [SerializeField]
        private string m_animationName;
        
        public override void HandleEnterState()
        {
            base.HandleEnterState();
            m_animator.Component.Play(m_animationName);
        }
    }
}