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
        
        private void Start()
        {
            _grassCache = new Dictionary<TerraVector, TerraGrassMonoView>();
            Game.Instance.GetViewModel<TerraViewModel>(0).OnGeometryUpdate += GeometryUpdate;
       }

        private void GeometryUpdate(TerraTerrainGeometryDataModel obj)
        {
            Render(Game.Instance.GetViewModel<TerraViewModel>(0));
        }

        private void Render(TerraViewModel vm)
        {
            System.Random rand = new System.Random();
            foreach (TerraGrassNodeGridPoint node in vm.Grass.AllData())
            {
                TerraGrassMonoView monoView = null;
                
                if (!_grassCache.TryGetValue(node.Vector, out monoView))
                {
                    GameObject instance = Instantiate(_grassView, transform, worldPositionStays: false);
                    monoView = instance.GetComponent<TerraGrassMonoView>();
                    instance.transform.position = vm.Geometry[node.Vector];
                    instance.transform.rotation = Quaternion.Euler(0,rand.Next(0, 360),0);
                    _grassCache.Add(node.Vector, monoView);
                }
                
                monoView.SetData(node.Data);
            }
        }
    }
}