using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;
using QFSW.MOP2;
using Terra.Utils;

namespace Terra.MonoViews
{
    public class TerraGrassWorldMonoView : MonoBehaviour
    {
        private Dictionary<TerraVector, TerraGrassMonoView> _grassCache;
        
        [SerializeField] 
        private GameObject _grassView;

        [SerializeField] 
        private int _radius = 10;

        private Transform _container;
        private ObjectPool _objectPool;
        private TerraViewModel _terraViewModel;

        private int _cacheWidth;
        private int _cacheHeight;
        
        private bool _hasInitializedStreaming;

        private TerraVector _lastRenderedPlayerPosition;
        
        private void Start()
        {
            _container = new GameObject($"{nameof(TerraGrassWorldMonoView)} GrassContainer").transform;
            _container.parent = transform;
            
            _objectPool = ObjectPool.Create(_grassView);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);

            _cacheWidth = _radius * 2 + 1;
            _cacheHeight = _radius * 2 + 1;
            _grassCache = new Dictionary<TerraVector, TerraGrassMonoView>();
            
            _terraViewModel.OnGeometryUpdate += GeometryUpdate;
            _terraViewModel.Grass.OnDataHasChanged += GrassOnOnDataHasChanged;
        }

        private void GrassOnOnDataHasChanged(IEnumerable<TerraGrassNodeGridPoint> data)
        {
            foreach (TerraGrassNodeGridPoint dataPoint in data)
            {
                UpdateGrass(dataPoint.Vector);
            }
        }

        private void UpdateGrass(TerraVector vector)
        {
            _grassCache.TryGetValue(vector, out TerraGrassMonoView currentGrassView);
            if (currentGrassView)
            {
                currentGrassView.SetData(vector, _terraViewModel.Grass[vector]);
            }
        }

        private void GeometryUpdate(TerraTerrainGeometryDataModel obj)
        {
            Render(Game.Instance.GetViewModel<TerraViewModel>(0));
        }

        private void Update()
        {
            if (_terraViewModel.PlayerEntity == null)
            {
                return;
            }
            
            TerraVector playerPosition = new TerraVector((int) _terraViewModel.PlayerEntity.Entity.Position.Data.x,
                (int)_terraViewModel.PlayerEntity.Entity.Position.Data.z);

            TerraVector localPlayerPosition = _terraViewModel.Chunk.WorldToLocal(playerPosition);
            
            if (!_hasInitializedStreaming)
            {
                _lastRenderedPlayerPosition = localPlayerPosition;
                _hasInitializedStreaming = true;
                AddGrass(new TerraArea(localPlayerPosition.x - _radius, localPlayerPosition.y + _radius, _radius * 2, _radius * 2));
            }
            else
            {
                bool shouldUpdateGrass = localPlayerPosition != _lastRenderedPlayerPosition;

                if (shouldUpdateGrass)
                {
                    UpdateGrass(localPlayerPosition);
                }
            }
        }
        
        private void UpdateGrassAreas(TerraVector playerPosition)
        {
            List<TerraArea> addAreas = null;
            List<TerraArea> removeAreas = null;
            
            TerraAreaUtils.CalculateChangeAreas(
                from:_lastRenderedPlayerPosition,
                to:playerPosition, 
                r:_radius,
                addAreas:out addAreas,
                removeAreas:out removeAreas);
            
            RemoveGrass(removeAreas);
            AddGrass(addAreas);
            
            _lastRenderedPlayerPosition = playerPosition;
        }
        
        private void RemoveGrass(IEnumerable<TerraArea> areas)
        {
            foreach (TerraArea area in areas)
            {
                RemoveGrass(area);
            }
        }

        private void RemoveGrass(TerraArea area)
        {
            foreach (TerraVector vector in GetVectors(area))
            {
                if (_grassCache.TryGetValue(vector, out TerraGrassMonoView grassView))
                {
                    _grassCache.Remove(vector);
                    _objectPool.Release(grassView.gameObject);
                }
            }
        }
        
        private void RemoveGrass(TerraVector vector)
        {
            if (_grassCache.TryGetValue(vector, out TerraGrassMonoView grassView))
            {
                _grassCache.Remove(vector);
                _objectPool.Release(grassView.gameObject);
            }
            
        }
        
        private void AddGrass(IEnumerable<TerraArea> areas)
        {
            foreach (TerraArea area in areas)
            {
                AddGrass(area);
            }
        }
        
        private void AddGrass(TerraArea area)
        {
            foreach (TerraVector vector in GetVectors(area))
            {
                bool shouldPlaceGrass = _terraViewModel.Grass[vector].Grass != 0;
                
                if (shouldPlaceGrass)
                {
                    AddGrass(vector);
                }
            }
        }

        private void AddGrass(TerraVector vector)
        {   
            System.Random rand = new System.Random(_terraViewModel.Chunk.LocalToWorld(vector).GetHashCode());
            _grassCache.Remove(vector);
            TerraGrassMonoView grassView = _objectPool.GetObject(
                _terraViewModel.Geometry[vector],
                Quaternion.Euler(0,rand.Next(0, 360),0)).GetComponent<TerraGrassMonoView>();
            grassView.SetData(vector, _terraViewModel.Grass[vector]);
            _grassCache.Add(vector, grassView);
        }

        private IEnumerable<TerraVector> GetVectors(TerraArea area)
        {
            for (int x = area.x; x < area.x + area.width; x++)
            {
                for (int y = area.y; y > area.y - area.height; y--)
                {
                    yield return new TerraVector(x, y);
                }
            }
        }

        private void Render(TerraViewModel vm)
        {
            /*foreach (TerraGrassNodeGridPoint node in vm.Grass.AllData())
            {
                TerraGrassMonoView monoView = null;
                
                if (!_grassCache.TryGetValue(node.Vector, out monoView))
                {
                    GameObject instance = Instantiate(_grassView, _container, worldPositionStays: false);
                    monoView = instance.GetComponent<TerraGrassMonoView>();
                    _grassCache.Add(node.Vector, monoView);
                }
                System.Random rand = new System.Random(vm.Chunk.LocalToWorld(node.Vector).GetHashCode());
                monoView.transform.rotation = Quaternion.Euler(0,rand.Next(0, 360),0);
                monoView.transform.position = vm.Geometry[node.Vector];
                monoView.SetData(node.Data);
            }*/
        }
    }
}