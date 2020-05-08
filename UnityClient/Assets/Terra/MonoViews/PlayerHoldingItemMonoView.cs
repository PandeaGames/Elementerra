using PandeaGames;
using Terra.MonoViews.Utility;
using Terra.SerializedData.GameState;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class PlayerHoldingItemMonoView : MonoBehaviour
    {
        private PlayerStateViewModel _vm;
        
        [SerializeField]
        private TerraEntityProxyMonoView _terraEntityProxyMonoView;
        
        private void Start()
        {
            _vm = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _vm.OnChange += VmOnOnChange;
            VmOnOnChange(_vm.State);
        }

        private void OnDestroy()
        {
            if (_vm != null)
            {
                _vm.OnChange -= VmOnOnChange;
            }
        }

        private void VmOnOnChange(TerraPlayerState obj)
        {
            _terraEntityProxyMonoView.Render(obj.HoldingInHandEntityId);
        }
    }
}