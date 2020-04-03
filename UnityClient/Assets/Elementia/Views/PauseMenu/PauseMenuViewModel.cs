using System;
using PandeaGames.ViewModels;

namespace Views.PauseMenu
{
    public class PauseMenuViewModel : IViewModel
    {
        public enum PauseMenuButtonIds
        {
            ContinueButton,
            MainMenuButton,
            ExitGameButton
        }

        public event Action<PauseMenuButtonIds> OnButtonPress;

        public void InvokeOnButtonPress(PauseMenuButtonIds id)
        {
            OnButtonPress?.Invoke(id);
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}