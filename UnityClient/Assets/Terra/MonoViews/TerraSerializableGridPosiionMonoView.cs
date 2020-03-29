using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;
using Random = System.Random;

namespace Terra.MonoViews
{
    public class TerraSerializableGridPosiionMonoView : AbstractTerraMonoComponent
    {

        [SerializeField]
        private Vector3 _offset;
        [SerializeField]
        private Vector3 _randomOffset;
        
        private TerraViewModel vm;
        protected override void Initialize(RuntimeTerraEntity entity)
        {
            base.Initialize(entity);
            vm = Game.Instance.GetViewModel<TerraViewModel>(0);
            vm.Geometry.OnDataHasChanged += GeometryOnDataHasChanged;
            
            Random rand = new Random(entity.InstanceId);
            
            Vector3 randomOffset = new Vector3(
                rand.Next((int)(Math.Min(0, _randomOffset.x) * 1000), (int)(Math.Max(0, _randomOffset.x) * 1000)) / (float)1000,
                rand.Next((int)(Math.Min(0, _randomOffset.x) * 1000), (int)(Math.Max(0, _randomOffset.x) * 1000)) / (float)1000,
                rand.Next((int)(Math.Min(0, _randomOffset.x) * 1000), (int)(Math.Max(0, _randomOffset.x) * 1000)) / (float)1000
                );
            
            transform.position = vm.Geometry.TryGetClosestGridPosition(entity.GridPosition.Data) + randomOffset + _offset;
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
            if (vm != null && vm.Geometry != null)
            {
                vm.Geometry.OnDataHasChanged -= GeometryOnDataHasChanged;
            }
        }
    }
}