using System;
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
    public class TerraLookTowardsPathState : AbstractPandeaState
    {
        [SerializeField]
        private TerraEntityPathContainer m_pathContainer;
        
        [SerializeField]
        private Vector3 m_adjustRotation;
        
        [SerializeField]
        private TransformReference m_target;

        private int m_indexOfPathNode;

        public override void HandleUpdateState()
        {
            base.HandleUpdateState();
            m_target.Component.LookAt(new Vector3(m_pathContainer.CurrentNodePosition.x, m_target.Component.position.y, m_pathContainer.CurrentNodePosition.z));
            m_target.Component.Rotate(m_adjustRotation);
        }
    }
}