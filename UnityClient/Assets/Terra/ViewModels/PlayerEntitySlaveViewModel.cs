using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.Entities;

namespace Terra.ViewModels
{
    public delegate void SlaveChangeDelegate(RuntimeTerraEntity current, RuntimeTerraEntity updated);
    
    public class PlayerEntitySlaveViewModel : IViewModel
    {
        public SlaveChangeDelegate OnSlaveChange;

        public RuntimeTerraEntity CurrentSlave { get; private set; }

        public void Reset()
        {
            
        }

        public void SetSlave(RuntimeTerraEntity entity)
        {
            OnSlaveChange?.Invoke(CurrentSlave, entity);
            CurrentSlave = entity;
        }
        
        public void ClearSlave()
        {
            SetSlave(null);
        }
    }
}