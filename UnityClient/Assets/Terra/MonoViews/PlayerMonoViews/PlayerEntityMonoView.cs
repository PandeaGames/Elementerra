using System;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews
{
    public class PlayerEntityMonoView : MonoBehaviour
    {
        private TerraViewModel _terraViewModel;
        private TerraWorldStateViewModel _worldStateViewModel;
        
        private void Start()
        {
            _worldStateViewModel =  Game.Instance.GetViewModel<TerraWorldStateViewModel>(0);
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _terraViewModel.RegisterEntity(GetComponent<TerraEntityMonoView>());
            UpdateDimension(_worldStateViewModel.IsWorldFipped);
            _worldStateViewModel.OnWorldFlipChange += UpdateDimension;
        }

        private void OnDestroy()
        {
            _worldStateViewModel.OnWorldFlipChange -= UpdateDimension;
        }

        private void Update()
        {
            TerraVector vector = _terraViewModel.Chunk.WorldToLocal(transform.position);
            _worldStateViewModel.IsWorldFipped = _terraViewModel.TerraAlterVerseViewModel[vector];
        }

        private void UpdateDimension(bool isWorldFlipped)
        {
            gameObject.layer = isWorldFlipped
                ? LayerMask.NameToLayer("BetaDimension")
                : LayerMask.NameToLayer("AlphaDimension");
        }
    }
}