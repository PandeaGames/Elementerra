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

        [SerializeField] private TerraEntityPathContainerReference m_pathReference;
        
        private TerraEntitiesViewModel m_terraEntitiesViewModel;

        private int m_indexOfPathNode;
        private List<TerraVector> m_path;
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
            m_pathReference.Component.Path = path;
        }

        private void OnDrawGizmosSelected()
        {
            if (m_path != null)
            {
                TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
                
                Gizmos.color = Color.blue;

                TerraVector toWorld = vm.Chunk.LocalToWorld(m_to);
                TerraVector fromWorld = vm.Chunk.LocalToWorld(m_from);
                Gizmos.DrawLine(new Vector3(toWorld.x, 0 ,toWorld.y), new Vector3(fromWorld.x, 0, fromWorld.y));
                
                Gizmos.color = Color.red;
                
                Gizmos.DrawLine(vm.Geometry[m_from], vm.Geometry[m_to]);

                if (TerraPlayerPrefs.TerraTerrainDebugViewType.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes
                    .WorldPositions))
                {
                    DebugUtils.DrawString($"({toWorld.x}:{toWorld.y})", vm.Geometry[m_to], Color.green, 10, 0, -10f);
                    DebugUtils.DrawString($"({fromWorld.x}:{fromWorld.y})", vm.Geometry[m_from], Color.green, 10, 0, -10f);
                } 
                else if (TerraPlayerPrefs.TerraTerrainDebugViewType.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes
                    .LocalPositions))
                {
                    DebugUtils.DrawString($"({m_to.x}:{m_to.y})", vm.Geometry[m_to], Color.green, 10, 0, -10f);
                    DebugUtils.DrawString($"({m_from.x}:{m_from.y})", vm.Geometry[m_from], Color.green, 10, 0, -10f);
                }
                
                Gizmos.color = Color.green;
               
                for (int i = 1; i < m_path.Count; i++)
                {
                    toWorld = vm.Chunk.LocalToWorld(m_path[i]);
                    fromWorld = vm.Chunk.LocalToWorld(m_path[i - 1]);
                    Gizmos.DrawLine(vm.Geometry[m_path[i]], vm.Geometry[m_path[i - 1]]);
                }
            }
        }   
    }
}