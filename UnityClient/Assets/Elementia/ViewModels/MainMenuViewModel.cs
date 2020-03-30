using System;
using System.Collections;
using System.Collections.Generic;
using PandeaGames.ViewModels;
using UnityEngine;

public class MainMenuViewModel : IViewModel
{
    public enum ButtonId
    {
        Continue,
        NewGame, 
        NewSandboxGame
    }

    public event Action<ButtonId> OnButtonPressed;

    public void TriggerButtonPress(ButtonId buttonId)
    {
        OnButtonPressed?.Invoke(buttonId);
    }

    public void Reset()
    {
        throw new System.NotImplementedException();
    }
}
