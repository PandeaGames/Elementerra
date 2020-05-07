using System;
using Terra.MonoViews.AI;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class TerraEntityMonoView : MonoBehaviour
    {
        public event Action<TerraEntityMonoView> OnViewDestroyed;
        public event Action<AttackDef> OnAttacked;
        
        public event Action<RuntimeTerraEntity> OnInitialize;
        private TerraEntitiesViewModel _viewModel;
        public bool IsInitialized { get; private set; }
        
        public RuntimeTerraEntity Entity { private set; get; }
        
        public void Initilize(RuntimeTerraEntity entity)
        {
            IsInitialized = true;
            Entity = entity;
            OnInitialize?.Invoke(entity);
        }

        private void OnDestroy()
        {
            OnViewDestroyed?.Invoke(this);
        }

        public void Attack(AttackDef attackDef)
        {
            Entity.Attack(attackDef);
            OnAttacked?.Invoke(attackDef);
        }
    }
}