using PandeaGames.ViewModels;
using Terra.SerializedData.Entities;

namespace Terra.ViewModels
{
    public delegate void EntityTargetChangeDelegate(RuntimeTerraEntity current, RuntimeTerraEntity updated);
    
    public class PlayerAttackTargetViewModel : IViewModel
    {
        public RuntimeTerraEntity CurrentTarget { get; private set; }
        
        public SlaveChangeDelegate OnChange;

        public void SetSlave(RuntimeTerraEntity entity)
        {
            OnChange?.Invoke(CurrentTarget, entity);
            CurrentTarget = entity;
        }
        
        public void ClearSlave()
        {
            SetSlave(null);
        } 
        
        public void Reset()
        {
            
        }
    }
}