using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.MonoViews.AI.References;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI.FSM
{
    public class TerraEntityPathContainer : MonoBehaviour
    {
        [SerializeField]
        private TerraEntityMonoViewReference m_terraEntityMonoView;
        
        private List<TerraVector> m_path;

        public List<TerraVector> Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        private bool _dirty;
        private TerraVector _closest;
        private void Update()
        {
            _dirty = true;
        }

        private TerraViewModel _vm;
        private void Awake()
        {
            _vm = Game.Instance.GetViewModel<TerraViewModel>();
        }

        public TerraVector ClosestLocalVector()
        {
            if (_dirty && m_path != null)
            {
                TerraVector localEntityPosition = _vm.Chunk.WorldToLocal(m_terraEntityMonoView.Component.Entity.Position.Data);
                float closestDist = float.MaxValue;

                foreach (TerraVector vector in m_path)
                {
                    float d = TerraVector.Distance(localEntityPosition, vector);
                    if (closestDist > TerraVector.Distance(localEntityPosition, vector))
                    {
                        _closest = vector;
                        closestDist = d;
                    }
                }
            }
            
            _dirty = false;
            return _closest;
        }
        
        public Vector3 ClosestWorldPosition()
        {
            return _vm.Geometry[ClosestLocalVector()];
        }

        private TerraVector _currentNode;
        /*public TerraVector GetNextLocalVector()
        {
            
        }*/
    }
}