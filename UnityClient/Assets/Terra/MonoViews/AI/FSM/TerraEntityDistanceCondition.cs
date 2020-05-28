using System;
using PandeaGames;
using PandeaGames.Runtime.Gameplay.AI;
using Terra.MonoViews.AI.References;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;
using Vector3 = System.Numerics.Vector3;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraEntityDistanceCondition : AbstractPandeaStateCondition
    {
        public enum RadiusType
        {
            OutsideRadius, 
            InsideRadius
        }
        
        [SerializeField]
        private float m_radiusInUnits;

        [SerializeField] 
        private RadiusType m_radiusType;

        [SerializeField]
        private TerraEntityMonoViewReference m_terraEntityMonoView;

        private TerraEntitiesViewModel m_terraEntitiesViewModel;

        private void Start()
        {
            m_terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
        }

        public override bool Evaluate(float secondsInCurrentState)
        {
            RuntimeTerraEntity thisEntity = m_terraEntityMonoView.Component.Entity;

            if (thisEntity != null)
            {
                string aggroLabel = thisEntity.EntityTypeData.AggroLabel;
                
                foreach (RuntimeTerraEntity entity in m_terraEntitiesViewModel.GetEntities(aggroLabel))
                {
                    float d = UnityEngine.Vector3.Distance(thisEntity.Position.Data, entity.Position.Data);

                    if (m_radiusType == RadiusType.InsideRadius && d < m_radiusInUnits)
                    {
#if UNITY_EDITOR
                        EvaluationLog.Add($"{nameof(TerraEntityDistanceCondition)} on {gameObject.name} [m_radiusType:{m_radiusType}, d:{d},. m_radiusInUnits:{m_radiusInUnits}, entity:{entity.EntityID}]");
#endif
                        return true;
                    }
                    else if(m_radiusType == RadiusType.OutsideRadius && d > m_radiusInUnits)
                    {
#if UNITY_EDITOR
                        EvaluationLog.Add($"{nameof(TerraEntityDistanceCondition)} on {gameObject.name} [m_radiusType:{m_radiusType}, d:{d},. m_radiusInUnits:{m_radiusInUnits}, entity:{entity.EntityID}]");
#endif
                        return true;
                    }
                }
            }
            
#if UNITY_EDITOR
                EvaluationLog.Add($"{nameof(TerraEntityDistanceCondition)} on {gameObject.name} [m_radiusType:{m_radiusType}, m_radiusInUnits:{m_radiusInUnits}");
#endif
            return false;
        }
    }
}