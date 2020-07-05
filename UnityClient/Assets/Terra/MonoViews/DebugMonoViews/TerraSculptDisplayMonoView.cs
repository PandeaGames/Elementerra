using System;
using PandeaGames;
using PandeaGames.Data;
using Terra.SerializedData.GameData;
using UnityEngine;

namespace Terra.MonoViews.DebugMonoViews
{
    public class TerraSculptDisplayMonoView : MonoBehaviour
    {
        
        private TerraDebugControlViewModel _controlViewModel;
        private TerraSculptViewModel _terraSculptViewModel;

        private string[] _options;
        private string[] _sculptOptions;
        private Vector2 _entitiesScrollArea;
        
        private void Start()
        {
            _sculptOptions = Enum.GetNames(typeof(TerraSculptViewModel.SculptMode));
            _options = Enum.GetNames(typeof(TerraDebugControlViewModel.States));
            _controlViewModel = Game.Instance.GetViewModel<TerraDebugControlViewModel>(0);
            _terraSculptViewModel = Game.Instance.GetViewModel<TerraSculptViewModel>(0);
        }
        
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            _controlViewModel.CurrentIndex =  GUILayout.Toolbar(_controlViewModel.CurrentIndex,_options);

            switch (_controlViewModel.CurrentState)
            {
                case TerraDebugControlViewModel.States.Sculpt:
                {
                    OnSculptGUI();
                    break;
                }
                case TerraDebugControlViewModel.States.PlaceEntity:
                {
                    OnPlaceEntityGUI();
                    break;
                }
            }
           
            GUILayout.EndVertical();
        }

        private void OnPlaceEntityGUI()
        {
            _entitiesScrollArea = GUILayout.BeginScrollView(_entitiesScrollArea);
            GUILayout.BeginVertical();
            foreach (TerraEntityTypeSO entityTypeSO in TerraGameResources.Instance.TerraEntityPrefabConfig.DataConfig)
            {
                if (entityTypeSO.Data.DebugImage == null)
                {
                    if (GUILayout.Button(entityTypeSO.name))
                    {
                        _controlViewModel.PlaceEntity(entityTypeSO.Data);
                    }
                }
                else
                {
                    if (GUILayout.Button(/*entityTypeSO.Data.DebugImage.texture, */entityTypeSO.name))
                    {
                        _controlViewModel.PlaceEntity(entityTypeSO.Data);
                    }
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
        }

        private void OnSculptGUI()
        {
            _terraSculptViewModel.CurrentIndex =  GUILayout.Toolbar(_terraSculptViewModel.CurrentIndex,_sculptOptions);
            GUILayout.Label("Size "+_terraSculptViewModel.Size);
            _terraSculptViewModel.Size = GUILayout.HorizontalSlider(_terraSculptViewModel.Size, TerraSculptViewModel.MinSize,
                TerraSculptViewModel.MaxSize);
            
            GUILayout.Label("Strength "+_terraSculptViewModel.Strength);
            _terraSculptViewModel.Strength = GUILayout.HorizontalSlider(_terraSculptViewModel.Strength, TerraSculptViewModel.MinStrength,
                TerraSculptViewModel.MaxStrength);
            
            GUILayout.Label("Flow "+_terraSculptViewModel.Flow);
            _terraSculptViewModel.Flow = GUILayout.HorizontalSlider(_terraSculptViewModel.Flow, TerraSculptViewModel.MinFlow,
                TerraSculptViewModel.MaxFlow);
        }
    }
}