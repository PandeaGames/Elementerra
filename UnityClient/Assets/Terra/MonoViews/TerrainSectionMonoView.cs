using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerrainSectionMonoView : MonoBehaviour
    {
        private TerraArea _localRenderArea;
        private TerraArea _localArea;
        private TerraTerrainGeometryDataModel _chunk;
        public void SetData(TerraTerrainGeometryDataModel chunk, TerraArea localRenderArea, TerraArea localArea)
        {
            _localArea = localArea;
            _localRenderArea = localRenderArea;
            _chunk = chunk;
        }
#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            /*for (int x = 0; x < _localRenderArea.width; x++)
            {
                for (int y = 0; y < _localRenderArea.height; y++)
                {
                    int localX = x + _localRenderArea.x;
                    int localY = y + _localRenderArea.y;
                    string text = $"({x}:{y})";
                    DebugUtils.DrawString(text,_chunk[localX, localY], Color.white);
                }
            }*/
        }
#endif
    }
}