using PandeaGames.Runtime.Gameplay.AI;
using UnityEngine;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraEntityDistanceCondition : AbstractPandeaStateCondition
    {
        [SerializeField]
        private float m_distanceInUnits;
        
        public override bool Evaluate(float secondsInCurrentState)
        {
            return false;
        }
    }
}