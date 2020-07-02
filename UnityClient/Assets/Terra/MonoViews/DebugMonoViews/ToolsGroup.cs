using PandeaGames;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class ToolsGroup : MonoBehaviour
    {
        private TerraDebugControlViewModel _controlViewModel;
        [SerializeField]
        private TerraSculptControls _sculpt;
        private void Start()
        {
            _controlViewModel = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
        }

        private void Update()
        {
            _sculpt.gameObject.SetActive(_controlViewModel.CurrentState == TerraDebugControlViewModel.States.Sculpt);
        }
    }
}