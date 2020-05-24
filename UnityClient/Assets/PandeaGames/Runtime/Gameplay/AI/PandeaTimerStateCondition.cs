using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaTimerStateCondition : AbstractPandeaStateCondition
    {
        [SerializeField] 
        private float m_secondsUntilStateChange;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            return secondsInCurrentState > m_secondsUntilStateChange;
        }
    }
}