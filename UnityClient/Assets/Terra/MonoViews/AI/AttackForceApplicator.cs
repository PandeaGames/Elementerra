using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class AttackForceApplicator : AbstractTerraMonoComponent
    {
        [SerializeField] private Rigidbody _rb;
        
        protected override void OnAttacked(AttackDef attackDef)
        {
            base.OnAttacked(attackDef);
            Debug.Log(gameObject.name + "HIT "+attackDef.Force);
            _rb.AddForce(attackDef.Force, ForceMode.VelocityChange);
            transform.position = new Vector3(transform.position.x, transform.position.y +1, transform.position.z);
        }
    }
}