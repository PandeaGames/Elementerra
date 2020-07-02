using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PandeaGames.ViewModels
{
    public abstract class AbstractStatefulViewModel<T> : AbstractViewModel where T:struct,IConvertible
    {
        public delegate void ViewModelStateDelegate(T state);
        public delegate void ViewModelStateChangeDelegate(T oldState, T newState);

        public event ViewModelStateDelegate OnLeaveState;
        public event ViewModelStateDelegate OnEnterState;
        public event ViewModelStateChangeDelegate OnStateChange;
        
        protected T _currentState;
        protected bool _canChangeState = true;

        public virtual T CurrentState
        {
            get { return _currentState; }
        }

        public virtual int CurrentIndex
        {
            get
            {
                string[] names = Enum.GetNames(typeof(T));

                for (int i = 0; i < names.Length; i++)
                {
                    if (names[i] == CurrentState.ToString())
                    {
                        return i;
                    }
                }
                
                return -1;
            }
            set
            {
                string[] names = Enum.GetNames(typeof(T));
                string name = names[value];
                T newValue = default(T);
                Enum.TryParse(name, out newValue);
                SetState(newValue);
            }
        }

        protected virtual void Start()
        {
            SetState(default(T), true);
        }

        protected virtual void SetState(T state, bool isInitialState)
        {
            if (_canChangeState && (!state.Equals(CurrentState) || isInitialState))
            {
                T oldState = CurrentState;
                LeaveState(CurrentState);
                _currentState = state;
                StateChange(oldState, CurrentState);
                EnterState(state);
            }
        }

        public void SetState(T state)
        {
            SetState(state, false);
        }

        protected virtual void LeaveState(T state)
        {
            if (OnLeaveState != null)
            {
                OnLeaveState(state);
            }
        }

        protected virtual void EnterState(T state)
        {
            if (OnEnterState != null)
            {
                OnEnterState(state);
            }
        }

        protected virtual void StateChange(T oldState, T newState)
        {
            if (OnStateChange != null)
            {
                OnStateChange(oldState, newState);
            }
        }
    }
}