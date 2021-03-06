﻿using UnityEngine;
using PandeaGames.Views;
using PandeaGames;
using PandeaGames.Data;
using PandeaGames.Views.Screens;

public class GameContainer : AbstractUnityView
{
    private const string ViewPath = "Prefabs/JunkyardGameView";
    
    private GameObject _unityView;
    private GameObject _unityViewPrefab;

    public override void Show()
    {
        _unityViewPrefab = GameResources.Instance.GameView;
        _unityView = GameObject.Instantiate(_unityViewPrefab, FindParentTransform());
        _window = _unityView.GetComponentInChildren<WindowView>();
        _serviceManager = _unityView.GetComponentInChildren<ServiceManager>();
    }
    
    public override void LoadAsync(LoadSuccess onLoadSuccess, LoadError onLoadError)
    {
        _unityViewPrefab = GameResources.Instance.GameView;
        onLoadSuccess?.Invoke();
    }
    
    public override Transform GetTransform()
    {
        return _unityView == null ? null:_unityView.transform;
    }
}