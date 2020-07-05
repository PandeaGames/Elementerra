using System;
using System.Collections.Generic;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;
using QFSW.MOP2;

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

        private TerraVector _lastRenderedPlayerPosition;
        
        private void Start()
        {
            _container = new GameObject($"{nameof(TerraGrassWorldMonoView)} GrassContainer").transform;
            _container.parent = transform;
            Game.Instance.GetViewModel<TerraViewModel>(0).OnGeometryUpdate += GeometryUpdate;
            _objectPool = ObjectPool.Create(_grassView);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);

            _cacheWidth = _radius * 2 + 1;
            _cacheHeight = _radius * 2 + 1;
            _grassCache = new Dictionary<TerraVector, TerraGrassMonoView>();
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

            bool shouldUpdateGrass = playerPosition != _lastRenderedPlayerPosition;

            if (shouldUpdateGrass)
            {
                UpdateGrass(playerPosition);
            }
        }

        private void UpdateGrass(TerraVector playerPosition)
        {
            List<TerraArea> addAreas = new List<TerraArea>();
            List<TerraArea> removeAreas = new List<TerraArea>();

            int deltaX = playerPosition.x - _lastRenderedPlayerPosition.x;
            int deltaY = playerPosition.y - _lastRenderedPlayerPosition.y;

            removeAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x - _radius, _lastRenderedPlayerPosition.y + _radius, Math.Max(0, deltaX), _cacheHeight));
            removeAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x + _radius + deltaX, _lastRenderedPlayerPosition.y + _radius, Math.Max(0, deltaX * -1), _cacheHeight));
            addAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x - _radius, _lastRenderedPlayerPosition.y + _radius, Math.Max(0, deltaX), _cacheHeight));
            addAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x + _radius + deltaX, _lastRenderedPlayerPosition.y + _radius, Math.Max(0, deltaX * -1), _cacheHeight));

            /*if (deltaY == 0)
            {
                removeAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x - _radius, _lastRenderedPlayerPosition.y - _radius, deltaX, _cacheHeight));
                addAreas.Add(new TerraArea(playerPosition.x + _radius - deltaX, playerPosition.y - _radius, deltaX, _cacheHeight));
            }
            else if(deltaY > 0)
            {
                removeAreas.Add(new TerraArea(_lastRenderedPlayerPosition.x - _radius, _lastRenderedPlayerPosition.y - _radius, deltaX, _cacheHeight));
                addAreas.Add(new TerraArea(playerPosition.x + _radius - deltaX, playerPosition.y - _radius, deltaX, _cacheHeight));
            }*/
            
            
            //if()
            
            _lastRenderedPlayerPosition = playerPosition;
        }
        
        private void RenderGrass(TerraVector position)
        {
            
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