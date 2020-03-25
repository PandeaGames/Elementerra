using System.Collections.Generic;
using PandeaGames;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraSerializableGridPosiionMonoView : AbstractTerraMonoComponent
    {
        private TerraViewModel vm;
        protected override void Initialize(RuntimeTerraEntity entity)
        {
            base.Initialize(entity);
            vm = Game.Instance.GetViewModel<TerraViewModel>(0);
            vm.Geometry.OnDataHasChanged += GeometryOnDataHasChanged;
            transform.position = vm.Geometry.TryGetClosestGridPosition(entity.GridPosition.Data);
        }
        
        private void Update()
        {
            if (Initialized)
            {
                Entity.GridPosition.Set(new TerraVector((int)transform.position.x, (int) transform.position.z));
            }
        }

        private void GeometryOnDataHasChanged(IEnumerable<TerraTerrainGeometryDataPoint> data)
        {
            foreach (TerraTerrainGeometryDataPoint point in data)
            {
               // if (point.Vector == (TerraVector) _entityMonoView.Entity.GridPosition.Data)
                //{
                    transform.position = vm.Geometry.TryGetClosestGridPosition(_entityMonoView.Entity.GridPosition.Data);
                //}
            }
        }

        public void OnDestroy()
        {
            vm.Geometry.OnDataHasChanged -= GeometryOnDataHasChanged;
        }
    }
}