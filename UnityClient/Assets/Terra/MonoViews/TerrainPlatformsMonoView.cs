using System.Collections;
using System.Collections.Generic;
using PandeaGames;
using Terra.Rendering;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerrainPlatformsMonoView : MonoBehaviour
    {
        private TerraViewModel _vm;
        private List<TerraTerrainBlobRenderer> _renderers { get; } = new List<TerraTerrainBlobRenderer>();

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(3);
            _vm = Game.Instance.GetViewModel<TerraViewModel>(0);
            RenderBlobs(_vm.TerraUniversBlobsViewModel.Blobs);
        }

        private void RenderBlobs(IEnumerable<TerraBlob> blobs)
        {
            foreach (TerraBlob blob in blobs)
            {
                _renderers.Add(RenderBlob(blob));
            }
        }

        private TerraTerrainBlobRenderer RenderBlob(TerraBlob blob)
        {
            TerraTerrainBlobRenderer renderer = new TerraTerrainBlobRenderer(blob, transform);
            renderer.Render();
            return renderer;
        }

        private void OnDrawGizmosSelected()
        {
            foreach (TerraTerrainBlobRenderer renderer in _renderers)
            {
                foreach (TerraVector localVector in renderer.Vertices)
                {
                    Gizmos.DrawSphere(_vm.Geometry[localVector], 0.2f);
                }
            }
        }
    }
}