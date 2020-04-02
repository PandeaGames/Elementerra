using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.GameData;
using Terra.SerializedData.GameState;

namespace Terra.ViewModels
{
    public class PlayerStateViewModel : IViewModel, ITerraEntityType
    {
        public event Action<TerraPlayerState> OnChange;
        
        public TerraPlayerState _terraPlayerState;
        public TerraPlayerState State
        {
            get => _terraPlayerState;
        }

        public bool IsHoldingItem => !string.IsNullOrEmpty(State.HoldingEntityID);
        

        public void Set(TerraPlayerState state)
        {
            _terraPlayerState = state;
        }
        
        public void SetHoldingEntityId(ITerraEntityType type)
        {
            SetHoldingEntityId(type.EntityID);
        }
        
        public void SetHoldingEntityId(string entityId)
        {
            _terraPlayerState.HoldingEntityID = entityId;
            OnChange?.Invoke(_terraPlayerState);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public string EntityID
        {
            get => State.HoldingEntityID;
        }
    }
}