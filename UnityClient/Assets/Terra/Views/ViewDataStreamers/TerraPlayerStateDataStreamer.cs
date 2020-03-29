using PandeaGames;
using Terra.SerializedData.GameState;
using Terra.Services;
using Terra.ViewModels;

namespace Terra.Views.ViewDataStreamers
{
    public class TerraPlayerStateDataStreamer : IDataStreamer
    {
        private PlayerStateViewModel _vm;
        private TerraDBService _db;
        private TerraPlayerStateService _stateService;
        
        public void Start()
        {
            _vm = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            _db = Game.Instance.GetService<TerraDBService>();
            _stateService = Game.Instance.GetService<TerraPlayerStateService>();
            _vm.Set(_stateService.GetPlayerState());
            _vm.OnChange += VmOnChange;
        }

        private void VmOnChange(TerraPlayerState state)
        {
            _stateService.WriteNewRecord(state);
        }

        public void Update(float time)
        {
            
        }

        public void Stop()
        {
            _vm.OnChange -= VmOnChange;
        }
    }
}