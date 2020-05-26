using System;
using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public delegate void StateEvent(AbstractPandeaState state);
    
    public abstract class AbstractPandeaState : MonoBehaviour
    {
        public event StateEvent OnEnterState;
        public event StateEvent OnLeaveState;

        [SerializeField]
        private PandeaStateTransition[] m_transitions;

        public virtual bool Evaluate(float secondsInCurrentState, out AbstractPandeaState newState)
        {
            foreach (PandeaStateTransition transition in m_transitions)
            {
                if (transition.Condition.Evaluate(secondsInCurrentState))
                {
                    newState = transition.AbstractPandeaState;
                    return true;
                }
            }

            newState = null;
            return false;
        }

        public virtual void HandleUpdateState()
        {
        }

        public virtual void HandleEnterState()
        {
            HandleState(OnEnterState);
        }
        
        public virtual void HandleLeaveState()
        {
            HandleState(OnLeaveState);
        }

        private void HandleState(StateEvent eventDelegate)
        {
            try
            {
                eventDelegate?.Invoke(this);
            }
            catch (Exception exception)
            {
                throw new Exception($"A problem was encountered when handling a new state in this state on {gameObject.name}.", innerException:exception);
            }
        }
    }
}