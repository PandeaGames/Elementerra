using PandeaGames;
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

        private void Start()
        {
            MainMenuViewModel vm = Game.Instance.GetViewModel<MainMenuViewModel>(0);
            _continueButton.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.Continue);
            });
            
            _newGame.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.Continue);
            });
            
            _newSandboxGame.onClick.AddListener(() =>
            {
                vm.TriggerButtonPress(MainMenuViewModel.ButtonId.Continue);
            });
        }
    }
}