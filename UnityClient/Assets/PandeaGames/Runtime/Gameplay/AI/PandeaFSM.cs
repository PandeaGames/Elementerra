using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public delegate void StateMachineEvent(AbstractPandeaState state, int stateIndex);
    
    public class PandeaFSM : MonoBehaviour
    {
        #if UNITY_EDITOR
        [HideInInspector]
        public List<string> StateChangeLog = new List<string>();
        #endif
        
        public event StateMachineEvent OnEnterState;
        public event StateMachineEvent OnLeaveState;
        
        [SerializeField] 
        private AbstractPandeaState[] m_states;

        private AbstractPandeaState m_currentState;
        private Dictionary<AbstractPandeaState, int> m_stateToIndexTable;
        private Dictionary<int, AbstractPandeaState> m_indexToStateTable;
        private float m_secondsWhenStateStarted;

        protected virtual void Start()
        {
            m_stateToIndexTable = new Dictionary<AbstractPandeaState, int>();
            m_indexToStateTable = new Dictionary<int, AbstractPandeaState>();
            
            for (int i = 0; i < m_states.Length; i++)
            {
                try
                {
                    AbstractPandeaState state = m_states[i];

                    state.OnEnterState += HandleEnterState;
                    state.OnLeaveState += HandleLeaveState;
                    
#if UNITY_EDITOR
                    state.OnLogTransition += OnLogTransition; 
#endif

                    m_stateToIndexTable.Add(state, i);
                    m_indexToStateTable.Add(i, state);
                }
                catch (NullReferenceException exception)
                {
                    throw new Exception("There was a problem setting up this state machine.\n Is there a null values in the states list?", innerException:exception);
                }
            }
            
            if (m_states.Length > 0)
            {
                m_currentState = m_states[0];
                m_currentState.HandleEnterState();
            }
        }

#if UNITY_EDITOR
        private void OnLogTransition(string log)
        {
            StateChangeLog.Add(log);
        }
#endif

        protected virtual void Update()
        {
            m_currentState?.HandleUpdateState();

            float secondsInCurrentState = Time.time - m_secondsWhenStateStarted;
            
            if (m_currentState != null && m_currentState.Evaluate(secondsInCurrentState, out AbstractPandeaState newState))
            {
                SetState(newState);
            }
        }

        protected virtual void SetState(AbstractPandeaState state)
        {
            if (m_currentState != null)
            {
                m_currentState.HandleLeaveState();
            }

            state.HandleEnterState();
            m_currentState = state;
            m_secondsWhenStateStarted = Time.time;
        }

        protected virtual void HandleEnterState(AbstractPandeaState state)
        {
            HandleState(state, OnEnterState);
        }
        
        protected virtual void HandleLeaveState(AbstractPandeaState state)
        {
            HandleState(state, OnLeaveState);
        }

        protected virtual void HandleState(AbstractPandeaState state, StateMachineEvent eventDelegate)
        {
            try
            {
                eventDelegate?.Invoke(state, m_stateToIndexTable[state]);
            }
            catch (NullReferenceException exception)
            {
                throw new Exception($"A problem was encountered when handling a new state in this state machine on {gameObject.name}. \nWas this called before the Start function of the MonoBehaviour?" +
                                    $"n Was this called after destruction?", innerException:exception);
            }
            catch (Exception exception)
            {
                throw new Exception($"A problem was encountered when handling a new state in this state machine on {gameObject.name}.", innerException:exception);
            }
        }
    }
}