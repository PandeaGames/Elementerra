using System;
using PandeaGames.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraSculptViewModel : AbstractSerializedStatefullViewModel<TerraSculptViewModel.SculptMode>
    {
        public const float MaxFlow = 1;
        public const float MinFlow = 0.1f;
        public const float MaxSize = 10;
        public const float MinSize = 1;
        public const float MaxStrength = 5;
        public const float MinStrength = 1;
        
        public enum SculptMode
        {
            Push = 0, 
            Pull = 1
        }
        
        public float Size
        {
            get { return PlayerPrefs.GetFloat("TerraSculptViewModel_Size", 1); }
            set { PlayerPrefs.SetFloat("TerraSculptViewModel_Size", value); }
        }
        
        public float Flow
        {
            get { return PlayerPrefs.GetFloat("TerraSculptViewModel_Flow", 1); }
            set { PlayerPrefs.SetFloat("TerraSculptViewModel_Flow", value); }
        }
        
        public float Strength
        {
            get { return PlayerPrefs.GetFloat("TerraSculptViewModel_Strength", 1); }
            set { PlayerPrefs.SetFloat("TerraSculptViewModel_Strength", value); }
        }
        
        public void Reset()
        {
            
        }
    }
}