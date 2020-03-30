using PandeaGames.Data;
using PandeaGames.Views;
using UnityEngine;

namespace Views
{
    public class MainMenuView : AbstractUnityView
    {
        private GameObject _view;
        
        public override void Destroy()
        {
            GameObject.Destroy(_view);
        }

        public override void Show()
        {
            _view = GameObject.Instantiate(TerraGameResources.Instance.MainMenuView);
        }
    }
}