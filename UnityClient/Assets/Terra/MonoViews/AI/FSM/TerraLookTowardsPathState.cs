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
        private TerraEntityPathContainerReference m_pathContainer;
        
        [SerializeField]
        private Vector3 m_adjustRotation;
        
        [SerializeField]
        private TransformReference m_target;

        private int m_indexOfPathNode;

        public override void HandleUpdateState()
        {
            base.HandleUpdateState();
            m_target.Component.LookAt(m_pathContainer.Component.ClosestWorldPosition());
            m_target.Component.Rotate(m_adjustRotation);
        }
    }
}