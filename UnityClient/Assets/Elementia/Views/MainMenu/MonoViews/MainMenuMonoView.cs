using PandeaGames;
using Terra.Services;
using UnityEngine;
using UnityEngine.UI;

namespace Views.MonoViews
{
    public class MainMenuMonoView : MonoBehaviour
    {
        [SerializeField] 
        public Button _continueButton;
        [SerializeField] 
        public Button _newGame;
        [SerializeField] 
        public Button _newSandboxGame;
        [SerializeField] 
        public Button _controlsButton;
        [SerializeField] 
        public Button _exitGameButton;

        private void Start()
        {
            MainMenuViewModel vm = Game.Instance.GetViewModel<MainMenuViewModel>(0);
            _continueButton.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.Continue);
            });
            
            _newGame.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.NewGame);
            });
            
            _newSandboxGame.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.NewSandboxGame);
            });
            
            _exitGameButton.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.ExitGame);
            });
            
            _controlsButton.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.Controls);
            });

            _continueButton.interactable = Game.Instance.GetService<TerraDBService>().HasUserSaveFile;
        }
    }
}