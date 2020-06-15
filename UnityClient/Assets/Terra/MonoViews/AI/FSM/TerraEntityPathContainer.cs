using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.MonoViews.AI.References;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraEntityPathContainer : MonoBehaviour
    {
        private static float NodeDistanceReached = 0.1f;
        
        [SerializeField]
        private TerraEntityMonoViewReference m_terraEntityMonoView;
        
        private List<TerraVector> m_path;

        public List<TerraVector> Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        private bool _dirty;
        private TerraVector _currentNode;
        private int _index;
        private void Update()
        {
            if (m_path == null || m_path.Count <= 0)
            {
                return;
            }
            
            _dirty = true;
            
            Vector3 entityPosition = m_terraEntityMonoView.Component.Entity.Position.Data;
            float closestDist = float.MaxValue;

            int index = 0;
            for (int i = 0; i < m_path.Count; i++)
            {
                TerraVector vector = m_path[i];
                float d = Vector3.Distance(entityPosition, _vm.Geometry[vector]);
                if (closestDist > d)
                {
                    _currentNode = vector;
                    closestDist = d;
                    index = i;
                }
            }

            _currentNode = m_path[Math.Min(index + 1, m_path.Count - 1)];
            
        }

        private TerraViewModel _vm;
        private void Awake()
        {
            _vm = Game.Instance.GetViewModel<TerraViewModel>(0);
        }

        public TerraVector CurrentLocalNode
        {
            get { return _currentNode; }
        }
        
        public TerraVector CurrentWorldNode
        {
            get { return _vm.Chunk.LocalToWorld(_currentNode); }
        }

        public Vector3 CurrentNodePosition
        {
            get { return _vm.Geometry[_currentNode]; }
        }
        
        private void OnDrawGizmosSelected()
        {
            if (m_path != null && m_path.Count > 0)
            {
                TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
                
                Gizmos.color = Color.blue;

                TerraVector to = m_path[m_path.Count - 1];
                TerraVector from = m_path[0];
                
                TerraVector toWorld = vm.Chunk.LocalToWorld(m_path[m_path.Count - 1]);
                TerraVector fromWorld = vm.Chunk.LocalToWorld(m_path[0]);
                Gizmos.DrawLine(new Vector3(toWorld.x, 0 ,toWorld.y), new Vector3(fromWorld.x, 0, fromWorld.y));

                if (TerraPlayerPrefs.TerraTerrainDebugViewType.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes
                    .WorldPositions))
                {
                    DebugUtils.DrawString($"({toWorld.x}:{toWorld.y})", vm.Geometry[to], Color.green, 10, 0, -10f);
                    DebugUtils.DrawString($"({fromWorld.x}:{fromWorld.y})", vm.Geometry[from], Color.green, 10, 0, -10f);
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