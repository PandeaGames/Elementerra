using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaTimerStateCondition : AbstractPandeaStateCondition
    {
        [SerializeField] 
        private float m_secondsUntilStateChange;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            bool evaluation = secondsInCurrentState > m_secondsUntilStateChange;
#if UNITY_EDITOR
            if (evaluation)
            {
                EvaluationLog.Add($"{nameof(PandeaTimerStateCondition)} on {gameObject.name} [secondsInCurrentState:{secondsInCurrentState}, m_chance:{m_secondsUntilStateChange}]");
            }
#endif
            return evaluation;
        }
    }
}