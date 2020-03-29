using PandeaGames;
using Terra.MonoViews.Utility;
using Terra.SerializedData.GameData;
using Terra.SerializedData.GameState;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class PlayerHoldingEntityMonoView : MonoBehaviour, ITerraEntityType
    {
        [SerializeField] 
        private TerraEntityProxyMonoView _terraEntityProxyMonoView;
        
        private PlayerStateViewModel _vm;
        private TerraPlayerState _currentState;
        
        private void Start()
        {
            _vm = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _vm.OnChange += VmOnChange;
            VmOnChange(_vm.State);
        }

        private void VmOnChange(TerraPlayerState newState)
        {
            TerraPlayerState oldState = _currentState;
            _currentState = newState;

            if (oldState.HoldingEntityID != newState.HoldingEntityID)
            {
                if (!string.IsNullOrEmpty(oldState.HoldingEntityID))
                {
                    _terraEntityProxyMonoView.Clear();
                }

                if (!string.IsNullOrEmpty(newState.HoldingEntityID))
                {
                    _terraEntityProxyMonoView.Render(this);
                }
            }
        }

        public string EntityID
        {
            get => _currentState.HoldingEntityID;
        }
    }
}