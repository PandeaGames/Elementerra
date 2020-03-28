using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.World;

namespace Terra.ViewModels
{
    public class TerraWorldStateViewModel : IViewModel
    {
        public event Action OnStateChange;
        private TerraWorldState _state;
        public TerraWorldState State
        {
            get => _state;
            private set => _state = value;
        }

        public void SetState(TerraWorldState state)
        {
            State = state;
        }

        public TerraWorldState Tick()
        {
            _state.Tick = State.Tick + 1;
            return State;
        }
        
        public void Reset()
        {
            
        }
    }
}