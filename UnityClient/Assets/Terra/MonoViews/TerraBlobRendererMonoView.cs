using System;
using System.Collections;
using System.Collections.Generic;
using PandeaGames;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraBlobRendererMonoView : MonoBehaviour
    {
        private TerraUniversBlobsViewModel _vm;
        private TerraViewModel _terraViewModel;
        private GameObject _root;
        
        [SerializeField]
        private Material _material;
        private IEnumerator Start()
        {
            _root = new GameObject("TerraBlobRendererMonoView");
            _root.transform.parent = transform.parent;
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _vm = _terraViewModel.TerraUniversBlobsViewModel;
            yield return new WaitForSeconds(5);
            List<TerraBlob> blobs = new List<TerraBlob>(_vm.Blobs);
            RenderBlobs(blobs);
        }

        public void OnDrawGizmos()
        {
            foreach (TerraBlob blob in _vm.Blobs)
            {
                Vector3 from = Vector3.negativeInfinity;
                foreach (TerraVector vertice in blob.Vertices)
                {
                    if (from == Vector3.negativeInfinity)
                    {
                        from = _terraViewModel.Geometry[vertice];
                        continue;
                    }
                    Gizmos.DrawLine(from, _terraViewModel.Geometry[vertice]);
                    from = _terraViewModel.Geometry[vertice];
                }
            }
        }

        private void Update()
        {
            
        }

        private void RenderBlobs(List<TerraBlob> blobs)
        {
            foreach (TerraBlob blob in blobs)
            {
                TerraBlobRenderer renderer = new TerraBlobRenderer();
                renderer.Render(blob, _root.transform, _material);
            }
        }
    }
}