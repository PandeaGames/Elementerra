using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.Entities;
using Terra.SerializedData.GameData;
using Terra.SerializedData.GameState;
using UnityEngine;

namespace Terra.ViewModels
{
    public class PlayerStateViewModel : IViewModel, ITerraEntityType
    {
        public event Action<TerraPlayerState> OnChange;
        
        private TerraPlayerState _terraPlayerState;
        public TerraPlayerState State
        {
            get => _terraPlayerState;
        }
        
        
        public bool IsHoldingItem => !string.IsNullOrEmpty(State.HoldingEntityID);
        public bool IsHoldingItemInHand => !string.IsNullOrEmpty(State.HoldingInHandEntityId) && State.HoldingInHandEntityId != "0";

        public void Set(TerraPlayerState state)
        {
            _terraPlayerState = state;
        }

        public void ClearHoldingEntityId()
        {
            SetHoldingEntityId(string.Empty, -1);
        }
        
        public void ClearHoldingInHandEntityId()
        {
            SetHoldingInHandEntityId(string.Empty, -1);
        }
        
        public void SetHoldingInHandEntityId(RuntimeTerraEntity entity)
        {
            SetHoldingInHandEntityId(entity.EntityID, entity.InstanceId);
        }
        
        public void SetHoldingInHandEntityId(string entityId, int instanceId)
        {
            _terraPlayerState.HoldingInHandEntityId = entityId;
            _terraPlayerState.HoldingInHandEntityInstanceId = instanceId;
            OnChange?.Invoke(_terraPlayerState);
        }
        
        public void SetHoldingEntityId(RuntimeTerraEntity entity)
        {
            SetHoldingEntityId(entity.EntityID, entity.InstanceId);
        }
        
        public void SetHoldingEntityId(string entityId, int instanceId)
        {
            _terraPlayerState.HoldingEntityID = entityId;
            _terraPlayerState.HoldingInstanceID = instanceId;
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