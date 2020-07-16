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
        public event Action<bool> OnWorldFlipChange;
        private TerraWorldState _state;
        public TerraWorldState State
        {
            get => _state;
            private set => _state = value;
        }

        private TimeOfDayData _timeOfDayData;

        public bool IsWorldFipped
        {
            get
            {
                return State.IsWorldFipped;
            }
            set 
            {
                if(_state.IsWorldFipped == value) return;
                _state.IsWorldFipped = value; 
                OnWorldFlipChange?.Invoke(value);
            }
        }
        
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
                if (_timeOfDayData == null)
                    return 0;
                
                int ticksPerDay = (int)( _timeOfDayData.DayLengthSeconds / TerraWorldStateStreamer.TickTimeSeconds);
                int ticksToday = State.Tick % ticksPerDay;
                return (float) ticksToday / ticksPerDay;
            }
        }

        public string GetTimeOfDayID()
        {
            for (int i = 0; i < _timeOfDayData.TimesOfDay.Length; i++)
            {
                if (i == _timeOfDayData.TimesOfDay.Length - 1)
                {
                    return _timeOfDayData.TimesOfDay[i].ID;
                }
                
                if (CurrentDayProgress > _timeOfDayData.TimesOfDay[i].Time && CurrentDayProgress < _timeOfDayData.TimesOfDay[i + 1].Time)
                {
                    return _timeOfDayData.TimesOfDay[i].ID;
                }
            }

            return string.Empty;
        }
    }
}