using System;
using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class TerraProjectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _force = 1;

        private AttackDef _attackDef;
        
        private void Update()
        {
            _rb.AddForce(transform.forward * _force, ForceMode.Force);
        }

        public void SetAttackDef(AttackDef attackDef)
        {
            _attackDef = attackDef;
        }

        private void OnCollisionEnter(Collision other)
        {
            TerraEntityMonoView entityMonoView = other.gameObject.GetComponent<TerraEntityMonoView>();

            if (entityMonoView != null)
            {
                entityMonoView.Attack(_attackDef);
            }
            
            Destroy(gameObject);
        }
    }
}