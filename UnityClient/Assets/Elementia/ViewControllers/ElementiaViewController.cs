using PandeaGames;
using PandeaGames.Data;
using PandeaGames.Views;
using PandeaGames.Views.ViewControllers;
using Terra.Services;
using Terra.ViewModels;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using Views.PauseMenu;
using TerraController = Terra.Controllers.TerraController;

namespace ViewControllers
{
    public enum ElementiaViewControllerStates
    {
        Preloading,
        MainMenu,
        Terra,
        ResetGame
    }
    
    public class ElementiaViewController : AbstractViewControllerFsm<ElementiaViewControllerStates>
    {
        private class ResetGameState : AbstractViewControllerState<ElementiaViewControllerStates>
        {
            
        }

        private class MainMenuState : AbstractViewControllerState<ElementiaViewControllerStates>
        {
            protected override IViewController GetViewController()
            {
                return new MainMenuViewController();
            }

            public override void EnterState(ElementiaViewControllerStates @from)
            {
                base.EnterState(@from);
                Game.Instance.Reset();
                Game.Instance.GetViewModel<MainMenuViewModel>(0).OnButtonPressed += OnButtonPressed;
            }

            public override void LeaveState(ElementiaViewControllerStates to)
            {
                base.LeaveState(to);
                Game.Instance.GetViewModel<MainMenuViewModel>().OnButtonPressed -= OnButtonPressed;
            }

            private void OnButtonPressed(MainMenuViewModel.ButtonId buttonId)
            {
                switch (buttonId)
                {
                    case MainMenuViewModel.ButtonId.NewGame:
                    {
                        Game.Instance.GetService<TerraDBService>().CopyGameDataToUserDataPath();
                        break;
                    }
                    case MainMenuViewModel.ButtonId.NewSandboxGame:
                    {
                        Game.Instance.GetService<TerraDBService>().DeleteCurrentUserData();
                        break;
                    }
                    case MainMenuViewModel.ButtonId.Controls:
                    {
                        ControlsDialogViewModel vm = Game.Instance.GetViewModel<ControlsDialogViewModel>();
                        DialogService.Instance.DisplayDialog<ControlsDialog>(vm);
                        break;
                    }
                    case MainMenuViewModel.ButtonId.ExitGame:
                    {
#if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
#else
 Application.Quit();    
#endif
                        break;
                    }
                }

                switch (buttonId)
                {
                    case MainMenuViewModel.ButtonId.Continue:
                    case MainMenuViewModel.ButtonId.NewSandboxGame:
                    case MainMenuViewModel.ButtonId.NewGame:
                    {
                        _fsm.SetState(ElementiaViewControllerStates.Terra);
                        break;
                    }
                }
            }
        }
        
        private class TerraState : AbstractViewControllerState<ElementiaViewControllerStates>
        {
            public override void EnterState(ElementiaViewControllerStates @from)
            {
                base.EnterState(@from);
                Game.Instance.GetViewModel<TerraEntitiesViewModel>(0).TerraEntityPrefabConfig =
                    ElementiaGameResources.Instance.TerraEntityPrefabConfig;
                
                Game.Instance.GetViewModel<PauseMenuViewModel>(0).OnButtonPress += OnButtonPress;
            }

            private void OnButtonPress(PauseMenuViewModel.PauseMenuButtonIds buttonId)
            {
                switch (buttonId)
                {
                    case PauseMenuViewModel.PauseMenuButtonIds.ExitGameButton:
                    {
#if UNITY_EDITOR
                        EditorApplication.isPlaying = false;
#else
 Application.Quit();    
#endif
                        break;
                    }
                    case PauseMenuViewModel.PauseMenuButtonIds.MainMenuButton:
                    {
                        _fsm.SetState(ElementiaViewControllerStates.MainMenu);
                        break;
                    }
                    case PauseMenuViewModel.PauseMenuButtonIds.Controls:
                    {
                        ControlsDialogViewModel vm = Game.Instance.GetViewModel<ControlsDialogViewModel>();
                        DialogService.Instance.DisplayDialog<ControlsDialog>(vm);
                        break;
                    }
                }
            }

            public override void LeaveState(ElementiaViewControllerStates to)
            {
                base.LeaveState(to);
                Game.Instance.GetViewModel<PauseMenuViewModel>(0).OnButtonPress -= OnButtonPress;
            }

            protected override IViewController GetViewController()
            {
                return new Terra.Controllers.TerraController();
            }
        }

        protected override IView CreateView()
        {
            return new GameContainer();
        }

        public ElementiaViewController()
        {
            SetViewStateController<TerraState>(ElementiaViewControllerStates.Terra);
            SetViewStateController<MainMenuState>(ElementiaViewControllerStates.MainMenu);
            SetViewStateController<ResetGameState>(ElementiaViewControllerStates.ResetGame);
            SetInitialState(ElementiaViewControllerStates.MainMenu);
        }
    }
}