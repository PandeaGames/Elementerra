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
                return roll < m_chance;
            }

            return false;
        }
    }
}