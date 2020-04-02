using System;
using PandeaGames;
using Terra.ViewModels;
using Terra.Views;
using UnityEngine;

namespace Terra.MonoViews
{
    public class PlayerEntityMonoView : MonoBehaviour
    {
        private TerraViewModel _terraViewModel;
        
        private void Start()
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
        }

        private void Update()
        {
            _terraViewModel.PlayerPosition = transform.position;
        }
    }
}