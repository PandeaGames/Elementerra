using System.Collections.Generic;
using PandeaGames;
using PandeaGames.Runtime.Gameplay.AI;
using PandeaGames.Services;
using Terra.MonoViews.AI.References;
using Terra.SerializedData.Entities;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraPathTargetSetter : AbstractPandeaState
    {
        [SerializeField]
        private TerraEntityMonoViewReference m_terraEntityMonoView;

        [SerializeField] private TerraEntityPathContainer m_path;
        
        private TerraEntitiesViewModel m_terraEntitiesViewModel;

        private int m_indexOfPathNode;
        private TerraVector m_to;
        private TerraVector m_from;

        public override void HandleEnterState()
        {
            base.HandleEnterState();
            m_terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            RuntimeTerraEntity thisEntity = m_terraEntityMonoView.Component.Entity;

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
                    TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
                    vm.Geometry.TryGetClosestGridPosition(thisEntity.Position.Data);
                    
                    m_from = vm.Chunk.WorldToLocal(new TerraVector((int) thisEntity.Position.Data.x,
                        (int)thisEntity.Position.Data.z));
                    m_to = vm.Chunk.WorldToLocal(new TerraVector((int)closestEntity.Position.Data.x,
                        (int) closestEntity.Position.Data.z));
                        
                    Game.Instance.GetService<PathfinderService>().GetPath(
                        PathFound,
                        vm.TerraPathfinderViewModel,
                        m_from,
                        m_to
                        );
                }
            }
        }

        private void PathFound(List<TerraVector> path)
        {
            m_path.Path = path;
        }

         
    }
}