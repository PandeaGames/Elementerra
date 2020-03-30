using PandeaGames;
using PandeaGames.Data;
using PandeaGames.Views.ViewControllers;
using Terra.Services;
using Terra.ViewModels;
using TerraController = Terra.Controllers.TerraController;

namespace ViewControllers
{
    public enum ElementiaViewControllerStates
    {
        Preloading,
        MainMenu,
        Terra
    }
    
    public class ElementiaViewController : AbstractViewControllerFsm<ElementiaViewControllerStates>
    {
        private class MainMenuState : AbstractViewControllerState<ElementiaViewControllerStates>
        {
            protected override IViewController GetViewController()
            {
                return new MainMenuViewController();
            }

            public override void EnterState(ElementiaViewControllerStates @from)
            {
                base.EnterState(@from);
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
                }
                
                _fsm.SetState(ElementiaViewControllerStates.Terra);
            }
        }
        
        private class TerraState : AbstractViewControllerState<ElementiaViewControllerStates>
        {
            public override void EnterState(ElementiaViewControllerStates @from)
            {
                base.EnterState(@from);
                Game.Instance.GetViewModel<TerraEntitiesViewModel>(0).TerraEntityPrefabConfig =
                    ElementiaGameResources.Instance.TerraEntityPrefabConfig;
            }

            protected override IViewController GetViewController()
            {
                return new Terra.Controllers.TerraController();
            }
        }
        
        public ElementiaViewController()
        {
            SetViewStateController<TerraState>(ElementiaViewControllerStates.Terra);
            SetViewStateController<MainMenuState>(ElementiaViewControllerStates.MainMenu);
            SetInitialState(ElementiaViewControllerStates.MainMenu);
        }
    }
}