using PandeaGames.ViewModels;
using Terra.SerializedData.GameData;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraDebugControlViewModel : AbstractStatefulViewModel<TerraDebugControlViewModel.States>
    {
        public enum States
        {
            None,
            PlaceEntity,
            Sculpt
        }
        
        public TerraEntityTypeData entityData;
        public Camera DebugCamera;
        
        public void PlaceEntity(TerraEntityTypeData entityData)
        {
            this.entityData = entityData;
            SetState(States.PlaceEntity, true);
        }
    }
}