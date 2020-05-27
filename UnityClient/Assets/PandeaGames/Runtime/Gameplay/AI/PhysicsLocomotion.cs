using System;
using System.Collections.Generic;
using UnityEngine;

namespace PandeaGames.Runtime.Gameplay.AI
{
    public class PhysicsLocomotion : AbstractPandeaLocomotion
    {
        [Serializable]
        public struct Config
        {
            [SerializeField] public ForceMode m_forceMode;
            [SerializeField] public bool m_relative;
        }
        
        [SerializeField] private RigidBodyReference m_rb;
        
        [SerializeField] private Config m_defaultConfig;
        [SerializeField] private Config[] m_additionalMoveConfigs;

        private List<Config> m_configs;

        private void Start()
        {
            m_configs = new List<Config>();
            m_configs.Add(m_defaultConfig);
            m_configs.AddRange(m_additionalMoveConfigs);
        }
        
        public override void SetRotation(Quaternion rotation)
        {
            m_transform.rotation = rotation;
        }

        public override void LootAt(Transform transform)
        {
            transform.LookAt(transform);
        }

        public override void Move(Quaternion direction, uint moveValue)
        {
            Move(direction.eulerAngles, moveValue);
        }

        public override void Move(Vector3 direction, uint moveValue)
        {
            Config config = m_configs[(int)Math.Max(m_configs.Count - 1, moveValue)];
            if (config.m_relative)
            {
                m_rb.Component.AddRelativeForce(direction, config.m_forceMode);
            }
            else
            {
                m_rb.Component.AddForce(direction, config.m_forceMode);
            }
        }
    }
}