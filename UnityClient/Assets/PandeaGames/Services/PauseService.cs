﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PandeaGames.Services;

public interface IPausable
{
    void Pause();
    void Resume();
}

public class PauseService : AbstractService<PauseService>
{
    private readonly List<IPausable> _pausables = new List<IPausable>();
    private bool _isPaused;

    public bool IsPaused
    {
        get => _isPaused;
    }

    public void RegisterPausable(IPausable pausable)
    {
        _pausables.Add(pausable);
    }
    
    public void UnregisterPausable(IPausable pausable)
    {
        _pausables.Remove(pausable);
    }

    public void Toggle()
    {
        if (_isPaused)
            Resume();
        else
            Pause();
    }

    public void Pause()
    {
        foreach(IPausable pausable in _pausables)
        {
            pausable.Pause();
        }

        _isPaused = true;
    }

    public void Pause(IPausable pausableFocus)
    {
        foreach (IPausable pausable in _pausables)
        {
            if(pausableFocus != pausable)
                pausable.Pause();
        }

        _isPaused = true;
    }

    public void Pause(List<IPausable> pausableFocus)
    {
        foreach (IPausable pausable in _pausables)
        {
            if (!pausableFocus.Contains(pausable))
                pausable.Pause();
        }

        _isPaused = true;
    }

    public void Resume()
    {
        foreach (IPausable pausable in _pausables)
        {
            pausable.Resume();
        }

        _isPaused = false;
    }
}
