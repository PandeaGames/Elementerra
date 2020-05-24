using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaPlayAnimationState : AbstractPandeaState
    {
        [SerializeField] 
        private Animator m_animator;

        [SerializeField]
        private string m_animationName;
        
        public override void HandleEnterState()
        {
            base.HandleEnterState();
            m_animator.Play(m_animationName);
        }
    }
}