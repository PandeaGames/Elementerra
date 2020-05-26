using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaStateGroup : AbstractPandeaState
    {
        [SerializeField] 
        private AbstractPandeaState[] m_subStates;
        
        public override void HandleUpdateState()
        {
            base.HandleUpdateState();
            
            foreach (AbstractPandeaState state in m_subStates)
            {
                state.HandleUpdateState();
            }
        }

        public override void HandleEnterState()
        {
            base.HandleEnterState();
            
            foreach (AbstractPandeaState state in m_subStates)
            {
                state.HandleEnterState();
            }
        }
        
        public override void HandleLeaveState()
        {
            base.HandleLeaveState();
            
            foreach (AbstractPandeaState state in m_subStates)
            {
                state.HandleLeaveState();
            }
        }
    }
}