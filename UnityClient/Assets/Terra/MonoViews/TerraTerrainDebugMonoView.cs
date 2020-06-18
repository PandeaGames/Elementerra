using PandeaGames;
using PandeaGames.ViewModels;
using Terra.Utils;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraTerrainDebugMonoView : MonoBehaviour
    {
        [SerializeField] private TerraPlayerPrefs.TerraTerrainDebugViewTypes _debugView;

        private void Awake()
        {
            _debugView = TerraPlayerPrefs.TerraTerrainDebugViewType;
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            TerraPlayerPrefs.TerraTerrainDebugViewType = _debugView;
        }

        private void OnDrawGizmos()
        {
            TerraViewModel vm = Game.Instance.GetViewModel<TerraViewModel>(0);
            
            if (_debugView.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes.Pathable))
            {
                foreach (BoolGridNode node in vm.TerraPathfinderViewModel.AllData())
                {
                    Gizmos.color = node.Data ? Color.green : Color.red;
                    Gizmos.DrawSphere(vm.Geometry[node.Vector], 0.15f);
                }
            }
            
            if (_debugView.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes.LocalPositions))
            {
                if (vm.Geometry!=null)
                {
                    foreach (TerraTerrainGeometryDataPoint node in vm.Geometry.AllData())
                    {
                        //TerraVector localPosition = 
                        string text = $"({node.Vector.x}:{node.Vector.y})";
                        DebugUtils.DrawString(text, node.Data, Color.white, 8);
                        /*Gizmos.color = Color.white;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(node.Data);
                        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
                        {
                            continue;
                        }
                        
                        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
                        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);*/
                    }
                }
            }
            
            if (_debugView.HasFlag(TerraPlayerPrefs.TerraTerrainDebugViewTypes.WorldPositions))
            {
                if (vm.Geometry!=null)
                {
                    foreach (TerraTerrainGeometryDataPoint node in vm.Geometry.AllData())
                    {
                        string text = $"({node.Data.x}:{node.Data.z})";
                        DebugUtils.DrawString(text, node.Data, Color.white, 8);
                        /*Gizmos.color = Color.white;
                        Vector3 screenPos = Camera.main.WorldToScreenPoint(node.Data);
                        if (screenPos.y < 0 || screenPos.y > Screen.height || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.z < 0)
                        {
                            continue;
                        }
                        
                        Vector2 size = GUI.skin.label.CalcSize(new GUIContent(text));
                        GUI.Label(new Rect(screenPos.x - (size.x / 2), -screenPos.y + view.position.height + 4, size.x, size.y), text);*/
                    }
                }
            }
        }
        
#endif
    }
}