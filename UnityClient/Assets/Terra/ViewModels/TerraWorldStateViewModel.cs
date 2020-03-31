using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.GameData;
using Terra.SerializedData.World;
using Terra.Views.ViewDataStreamers;

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

        private TimeOfDayData _timeOfDayData;

        public void SetConfig(TimeOfDayData timeOfDayData)
        {
            _timeOfDayData = timeOfDayData;
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

        public float CurrentDayProgress
        {
            get
            {
                int ticksPerDay = (int)( _timeOfDayData.DayLengthSeconds / TerraWorldStateStreamer.TickTimeSeconds);
                int ticksToday = State.Tick % ticksPerDay;
                return (float) ticksToday / ticksPerDay;
            }
        }
    }
}