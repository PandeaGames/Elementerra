using System.Collections.Generic;
using PandeaGames;
using Terra.SerializedData.Entities;
using Terra.SerializedData.World;
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
        private TerraArea _currentArea;
        private void Start()
        {
            _views = new Dictionary<int, TerraEntityMonoView>();
            _terraEntitiesViewModel = Game.Instance.GetViewModel<TerraEntitiesViewModel>(0);
            _terraEntitiesViewModel.OnAddEntity += OnAddEntity;
            _terraEntitiesViewModel.OnRemoveEntity += TerraEntitiesViewModelOnRemoveEntity;
            _terraEntityPrefabConfig = _terraEntitiesViewModel.TerraEntityPrefabConfig;
            Game.Instance.GetViewModel<TerraChunksViewModel>(0).OnChunkAdded += OnChunkAdded;
            _currentArea = Game.Instance.GetViewModel<TerraChunksViewModel>(0).CurrentArea;
        }

        private void OnChunkAdded(TerraWorldChunk chunk)
        {
            Debug.Log("Chunk Loaded: "+chunk.Area);
            _currentArea = chunk.Area;
            foreach (TerraEntityMonoView entityMonoView in _views.Values)
            {
                entityMonoView.gameObject.SetActive(_currentArea.Contains(entityMonoView.transform.position, 4));
            }
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
            entity.SetActive(_currentArea.Contains(obj.Position.Data, 4));
        }
    }
}