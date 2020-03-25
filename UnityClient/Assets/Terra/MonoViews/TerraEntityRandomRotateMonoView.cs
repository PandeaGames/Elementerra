using Terra.SerializedData.Entities;
using UnityEngine;
using Random = System.Random;
namespace Terra.MonoViews
{
    public class TerraEntityRandomRotateMonoView : AbstractTerraMonoComponent
    {
        [SerializeField]
        private Vector3 _range;
        
        protected override void Initialize(RuntimeTerraEntity Entity)
        {
            base.Initialize(Entity);
            Random ran = new Random(Entity.InstanceId);

            Vector3 current = transform.rotation.eulerAngles;
            Vector3 randomVector = new Vector3(
                (float)ran.Next((int)(_range.x * 1000)) / 1000,
                (float)ran.Next((int)(_range.y * 1000)) / 1000,
                (float)ran.Next((int)(_range.z * 1000)) / 1000
                );

            Vector3 newVector = current + randomVector;
            transform.rotation = Quaternion.Euler(newVector);
        }
    }
}