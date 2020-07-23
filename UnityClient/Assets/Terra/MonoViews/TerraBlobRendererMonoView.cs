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
        private IEnumerator Start()
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _vm = _terraViewModel.TerraUniversBlobsViewModel;
            yield return new WaitForSeconds(5);
            List<TerraBlob> blobs = new List<TerraBlob>(_vm.Blobs);
        }

        public void OnDrawGizmos()
        {    
            Vector3 from = Vector3.negativeInfinity;
            foreach (TerraBlob blob in _vm.Blobs)
            {
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
    }
}