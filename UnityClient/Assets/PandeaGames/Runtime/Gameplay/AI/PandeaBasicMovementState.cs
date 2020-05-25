using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaBasicMovementState : AbstractPandeaState
    {
        [SerializeField] 
        private Vector3 m_direction;

        [SerializeField] private Rigidbody m_rb;
        [SerializeField] private ForceMode m_forceMode;
        [SerializeField] private bool m_relative;

        public override void HandleUpdateState()
        {
            base.HandleUpdateState();
            
            if (m_relative)
            {
                m_rb.AddRelativeForce(m_direction, m_forceMode);
            }
            else
            {
                m_rb.AddForce(m_direction, m_forceMode);
            }
        }
    }
}