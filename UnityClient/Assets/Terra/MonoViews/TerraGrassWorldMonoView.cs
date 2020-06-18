using System.Collections.Generic;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraGrassWorldMonoView : MonoBehaviour
    {
        private Dictionary<TerraVector, TerraGrassMonoView> _grassCache;
        
        [SerializeField] 
        private GameObject _grassView;

        private Transform _container;
        
        private void Start()
        {
            _container = new GameObject($"{nameof(TerraGrassWorldMonoView)} GrassContainer").transform;
            _container.parent = transform;
            _grassCache = new Dictionary<TerraVector, TerraGrassMonoView>();
            Game.Instance.GetViewModel<TerraViewModel>(0).OnGeometryUpdate += GeometryUpdate;
       }

        private void GeometryUpdate(TerraTerrainGeometryDataModel obj)
        {
            Render(Game.Instance.GetViewModel<TerraViewModel>(0));
        }

        private void Render(TerraViewModel vm)
        {
           /* foreach (TerraGrassNodeGridPoint node in vm.Grass.AllData())
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