using PandeaGames;
using PandeaGames.Runtime.Gameplay.AI;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraTimeOfDayCondition : AbstractPandeaStateCondition
    {
        [SerializeField]
        private string m_timeOfDayId;
        
        [SerializeField]
        private bool m_negateCondition;
        
        private TerraWorldStateViewModel m_worldState;

        public void Start()
        {
            m_worldState = Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
        }
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            string timeOfDay = m_worldState.GetTimeOfDayID();
            bool evaluation = m_negateCondition ? timeOfDay != m_timeOfDayId : timeOfDay == m_timeOfDayId;
            
#if UNITY_EDITOR
            EvaluationLog.Add($"{nameof(TerraTimeOfDayCondition)} on {gameObject.name} [m_timeOfDayId:{m_timeOfDayId}, m_negateCondition:{m_negateCondition}, timeOfDay:{timeOfDay}, evaluation{evaluation}");
#endif
            
            return evaluation;
        }
    }
}