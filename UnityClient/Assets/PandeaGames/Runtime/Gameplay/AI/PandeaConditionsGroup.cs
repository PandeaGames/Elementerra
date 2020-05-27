using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaConditionsGroup : AbstractPandeaStateCondition
    {
        [SerializeField] private AbstractPandeaStateCondition[] m_conditions;
        [SerializeField] private bool m_mustBeUnanimous;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            foreach (AbstractPandeaStateCondition condition in m_conditions)
            {
                bool evaluation = condition.Evaluate(secondsInCurrentState);
                if (!evaluation && m_mustBeUnanimous)
                {
                    return false;
                }
                else if(evaluation && !m_mustBeUnanimous)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}