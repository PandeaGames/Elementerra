using System;
using UnityEngine;

namespace PandeaGames.ViewModels
{
    public abstract class AbstractSerializedStatefullViewModel<T> : AbstractStatefulViewModel<T>
        where T : struct, IConvertible
    {
        private string Key => GetType().ToString();

        public override T CurrentState
        {
            get
            {
                string[] names = Enum.GetNames(typeof(T));
                string name = names[PlayerPrefs.GetInt(Key, 0)];
                T state = default(T);
                Enum.TryParse(name, out state);
                return state;
            }
        }

        protected override void EnterState(T state)
        {
            base.EnterState(state);
            string[] names = Enum.GetNames(typeof(T));

            for (int i = 0; i < names.Length; i++)
            {
                if (names[i] == state.ToString())
                {
                    PlayerPrefs.SetInt(Key, i);
                }
            }
        }
    }
}