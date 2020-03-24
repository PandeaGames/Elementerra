using PandeaGames;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraSerializableGridPosiionMonoView : AbstractTerraMonoComponent
    {
        protected override void Initialize(RuntimeTerraEntity entity)
        {
            base.Initialize(entity);
            TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
            transform.position = new Vector3(entity.GridPosition.Data.x, vm.Geometry[entity.GridPosition.Data].y, entity.GridPosition.Data.y);
        }
    }
}