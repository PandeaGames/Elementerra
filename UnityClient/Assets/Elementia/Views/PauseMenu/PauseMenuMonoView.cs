using System;
using PandeaGames;
using UnityEngine;
using UnityEngine.UI;

namespace Views.PauseMenu
{
    public class PauseMenuMonoView : MonoBehaviour, IPausable
    {
        [SerializeField] 
        private Button _exitGameButton;
        
        [SerializeField] 
        private Button _toMainMenuButton;
        
        [SerializeField]
        private Button _returnButton;
        [SerializeField]
        private Button _controlsButton;

        [SerializeField] 
        private GameObject _menuContainer;

        private PauseService _pauseService;

        private void Start()
        {
            _pauseService = Game.Instance.GetService<PauseService>();
            _pauseService.RegisterPausable(this);
            _menuContainer.SetActive(_pauseService.IsPaused);
            PauseMenuViewModel vm = Game.Instance.GetViewModel<PauseMenuViewModel>(0);
            _exitGameButton.onClick.AddListener(() =>
            {
                vm.InvokeOnButtonPress(PauseMenuViewModel.PauseMenuButtonIds.ExitGameButton);
            });
            _toMainMenuButton.onClick.AddListener(() =>
            {
                vm.InvokeOnButtonPress(PauseMenuViewModel.PauseMenuButtonIds.MainMenuButton);
            });
            
            _controlsButton.onClick.AddListener(() =>
            {
                vm.InvokeOnButtonPress(PauseMenuViewModel.PauseMenuButtonIds.Controls);
            });
            _returnButton.onClick.AddListener(() =>
            {
                _pauseService.Toggle();
            });
        }

        private void OnDestroy()
        {
            _pauseService.UnregisterPausable(this);
            _pauseService = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _pauseService.Toggle();
            }
        }

        public void Pause()
        {
            _menuContainer.SetActive(true);
        }

        public void Resume()
        {
            _menuContainer.SetActive(false);
        }
    }
}