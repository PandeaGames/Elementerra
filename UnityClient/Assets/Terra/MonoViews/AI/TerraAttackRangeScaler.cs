using Terra.SerializedData.Entities;
using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class TerraAttackRangeScaler : AbstractTerraMonoComponent
    {
        [SerializeField] private Transform _attackColliderTransform;
        
        // Start is called before the first frame update
        protected override void Initialize(RuntimeTerraEntity Entity)
        {
            base.Initialize(Entity);

            _attackColliderTransform.localScale = new Vector3(
                Entity.EntityTypeData.AttackRange,
                Entity.EntityTypeData.AttackRange,
                Entity.EntityTypeData.AttackRange
            );
        }
    }
}