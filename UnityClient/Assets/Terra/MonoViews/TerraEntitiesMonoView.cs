using System.Collections.Generic;
using PandeaGames;
using Terra.SerializedData.Entities;
using Terra.StaticData;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraEntitiesMonoView : MonoBehaviour
    {
        private TerraEntityPrefabConfig _terraEntityPrefabConfig;
        private TerraEntitiesViewModel _terraEntitiesViewModel;
        private Dictionary<int, TerraEntityMonoView> _views;
        private void Start()
        {
            _views = new Dictionary<int, TerraEntityMonoView>();
            _terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            _terraEntitiesViewModel.OnAddEntity += OnAddEntity;
            _terraEntitiesViewModel.OnRemoveEntity += TerraEntitiesViewModelOnRemoveEntity;
            _terraEntityPrefabConfig = _terraEntitiesViewModel.TerraEntityPrefabConfig;
        }

        private void TerraEntitiesViewModelOnRemoveEntity(RuntimeTerraEntity obj)
        {
            Destroy(_views[obj.InstanceId].gameObject);
            _views.Remove(obj.InstanceId);
        }

        private void OnDestroy()
        {
            _terraEntitiesViewModel.OnAddEntity -= OnAddEntity;
            _terraEntitiesViewModel.OnRemoveEntity -= TerraEntitiesViewModelOnRemoveEntity;
        }

        private void OnAddEntity(RuntimeTerraEntity obj)
        {
            GameObject entity = Instantiate(_terraEntityPrefabConfig.GetGameObject(obj), transform);
            entity.GetComponent<TerraEntityMonoView>().Initilize(obj);
            _views.Add(obj.InstanceId, entity.GetComponent<TerraEntityMonoView>());
        }
    }
}