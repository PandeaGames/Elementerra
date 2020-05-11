using System;
using PandeaGames.ViewModels;
using Terra.SerializedData.GameData;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public delegate void WorldContextDelegate(Vector3 transform, WorldContextViewModel.Context context, ITerraEntityType data);
    
    public class WorldContextViewModel : IViewModel
    {
        public event WorldContextDelegate OnChange;
        
        public enum Context
        {
            None,
            PickUp,
            PutInInventory,
            Holding,
            Enslave,
            Build
        }
        
        public Context CurrentContext { get; private set; }
        public Vector3 CurrentTransform { get; private set; }
        public ITerraEntityType Data { get; set; }
        
        public void ClearContext()
        {
            CurrentContext = Context.None;
            OnChange?.Invoke(default(Vector3), CurrentContext, null);
        }

        public void SetContext(Vector3 transform, Context context, ITerraEntityType data)
        {
            CurrentContext = context;
            CurrentTransform = transform;
            OnChange?.Invoke(transform, context, data);
        }
        
        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}