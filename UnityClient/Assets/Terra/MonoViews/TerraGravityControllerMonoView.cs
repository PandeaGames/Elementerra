using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraGravityControllerMonoView : MonoBehaviour
    {
        [Serializable]
        public struct Settings
        {
            public float Mass;
            public float Gravity;
            public float JumpForce;
        }
        
        public event System.Action Jumped;
        
        [SerializeField]
        public GroundCheck groundCheck;

        public Settings BaseSettings;
        public Settings BetaSettings;
        [SerializeField] private Rigidbody _rb;

        private TerraViewModel _vm;

        private void Start()
        {
            _vm = Game.Instance.GetViewModel<TerraViewModel>(0);
        }

        public void Update()
        {
            TerraVector vector = _vm.Chunk.WorldToLocal(transform.position);

            Settings settings = _vm.TerraAlterVerseViewModel[vector] ? BetaSettings : BaseSettings;

            _rb.mass = settings.Mass;
            
            _rb.AddForce(new Vector3(0, settings.Gravity, 0), ForceMode.Acceleration);
        }
        
        void LateUpdate()
        {
            TerraVector vector = _vm.Chunk.WorldToLocal(transform.position);
            Settings settings = _vm.TerraAlterVerseViewModel[vector] ? BetaSettings : BaseSettings;
            
            if (Input.GetButtonDown("Jump") && groundCheck.isGrounded)
            {
                _rb.AddForce(Vector3.up * 100 * settings.JumpForce);
                Jumped?.Invoke();
            }
        }
    }
}