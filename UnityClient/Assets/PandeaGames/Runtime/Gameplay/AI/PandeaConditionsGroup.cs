using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaConditionsGroup : AbstractPandeaStateCondition
    {
        [SerializeField] 
        private AbstractPandeaStateCondition[] m_conditions;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            foreach (AbstractPandeaStateCondition condition in m_conditions)
            {
                if (!condition.Evaluate(secondsInCurrentState))
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}