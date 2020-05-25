using PandeaGames;
using PandeaGames.Runtime.Gameplay.AI;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraEntityLookTowardsState : AbstractPandeaState
    {
        [SerializeField]
        private TerraEntityMonoView m_terraEntityMonoView;
        
        [SerializeField]
        private Vector3 m_adjustRotation;

        private TerraEntitiesViewModel m_terraEntitiesViewModel;

        private void Start()
        {
            m_terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
        }
        
        public override void HandleUpdateState()
        {
            base.HandleUpdateState();
            
            RuntimeTerraEntity thisEntity = m_terraEntityMonoView.Entity;

            if (thisEntity != null)
            {
                string aggroLabel = thisEntity.EntityTypeData.AggroLabel;
                float closestDist = float.MaxValue;
                RuntimeTerraEntity closestEntity = null;
                foreach (RuntimeTerraEntity entity in m_terraEntitiesViewModel.GetEntities(aggroLabel))
                {
                    float d = Vector3.Distance(thisEntity.Position.Data, entity.Position.Data);

                    if (d < closestDist)
                    {
                        closestEntity = entity;
                        closestDist = d;
                    }
                }

                if (closestEntity != null)
                {
                    transform.LookAt(closestEntity.Position.Data);
                    transform.Rotate(m_adjustRotation);
                }
            }
        }
    }
}