using Terra.SerializedData.Entities;
using UnityEngine;
using Random = System.Random;

namespace Terra.MonoViews
{
    public class TerraEntityRandomScaleMonoView : AbstractTerraMonoComponent
    {
        [SerializeField, Range(0, 5)]
        private float _range;
        
        protected override void Initialize(RuntimeTerraEntity Entity)
        {
            base.Initialize(Entity);
            Random ran = new Random(Entity.InstanceId);
            float scale = (float) ran.Next((int)(_range * 1000))/1000;
            transform.localScale = new Vector3(1+scale, 1+scale, 1+scale);
        }
    }
}