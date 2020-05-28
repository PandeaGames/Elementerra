using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PandeaRandomCondition : AbstractPandeaStateCondition
    {
        [SerializeField] private float m_evaluateSecondsInterval;
        [SerializeField, Range(0, 100)] private int m_chance;

        private float m_lastEvaluationSeconds;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            if (m_lastEvaluationSeconds + m_evaluateSecondsInterval < Time.time)
            {
                int roll = Random.Range(0, 100);
                m_lastEvaluationSeconds = Time.time;
                bool evaluation = roll < m_chance;
                
#if UNITY_EDITOR
                EvaluationLog.Add($"{nameof(PandeaRandomCondition)} on {gameObject.name} [roll:{roll}, m_chance:{m_chance}, evaluation:{evaluation}]");     
#endif
                
                return evaluation;
            }

            return false;
        }
    }
}