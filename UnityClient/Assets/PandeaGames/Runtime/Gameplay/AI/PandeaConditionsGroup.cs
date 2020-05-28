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
#if UNITY_EDITOR
                    EvaluationLog.Add($"{nameof(PandeaConditionsGroup)} on {gameObject.name} Condition on object {condition.gameObject} of type {condition.GetType()} was not met.");
#endif
                    return false;
                }
                else if(evaluation && !m_mustBeUnanimous)
                {
#if UNITY_EDITOR
                    EvaluationLog.Add($"{nameof(PandeaConditionsGroup)} on {gameObject.name} Condition on object {condition.gameObject} of type {condition.GetType()} met.");
#endif
                    return true;
                }
            }
            
            bool isUnanimous = m_conditions.Length > 0 && m_mustBeUnanimous;
            
#if UNITY_EDITOR
            EvaluationLog.Add($"{nameof(PandeaConditionsGroup)} All conditions met");
#endif
            
            return isUnanimous;
        }
    }
}