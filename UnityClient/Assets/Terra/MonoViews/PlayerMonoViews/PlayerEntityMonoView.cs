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
            _terraViewModel.RegisterEntity(GetComponent<TerraEntityMonoView>());
        }
    }
}