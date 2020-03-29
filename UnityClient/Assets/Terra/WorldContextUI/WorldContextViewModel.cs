using System;
using PandeaGames.ViewModels;
using UnityEngine;

namespace Terra.WorldContextUI
{
    public delegate void WorldContextDelegate(Vector3 transform, WorldContextViewModel.Context context, int data);
    
    public class WorldContextViewModel : IViewModel
    {
        public event WorldContextDelegate OnChange;
        
        public enum Context
        {
            None,
            PickUp
        }
        
        public Context CurrentContext { get; private set; }
        public Vector3 CurrentTransform { get; private set; }
        public int Data { get; set; }
        
        public void ClearContext()
        {
            CurrentContext = Context.None;
            OnChange?.Invoke(default(Vector3), CurrentContext, 0);
        }

        public void SetContext(Vector3 transform, Context context, int data)
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