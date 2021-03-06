﻿using System.Collections;
using System.Collections.Generic;
using PandeaGames;
using UnityEngine;

public class Pausable : MonoBehaviour, IPausable {

    public delegate void OnPauseDelegate();

    public event OnPauseDelegate OnPause;
    public event OnPauseDelegate OnResume;

    private PauseService _pauseService;

    private List<Rigidbody> _rigidbodies = new List<Rigidbody>();
    private List<Rigidbody2D> _rigidbodies2D = new List<Rigidbody2D>();
    private List<Animator> _animations = new List<Animator>();

    private Dictionary<Rigidbody, bool> _rigidBodyKinematicStates = new Dictionary<Rigidbody, bool>();
    private Dictionary<Rigidbody, bool> _rigidBodyCollisionStates = new Dictionary<Rigidbody, bool>();
    private Dictionary<Rigidbody2D, bool> _rigidBodyKinematic2DStates = new Dictionary<Rigidbody2D, bool>();
    private Dictionary<Rigidbody2D, bool> _rigidBodySimulated2DStates = new Dictionary<Rigidbody2D, bool>();
    private Dictionary<Animator, bool> _animationStates = new Dictionary<Animator, bool>();
    private Dictionary<Component, bool> _componentStates = new Dictionary<Component, bool>();

    [SerializeField] 
    private List<MonoBehaviour> _components;
    
    private List<Component> _componentsToPause;

    // Use this for initialization
    private void Start ()
    {
        _componentsToPause = new List<Component>();
        _pauseService = Game.Instance.GetService<PauseService>();
        _pauseService.RegisterPausable(this);

        GetComponentsInChildren<Rigidbody>(true, _rigidbodies);
        GetComponents<Rigidbody>(_rigidbodies);

        GetComponentsInChildren<Rigidbody2D>(true, _rigidbodies2D);
        GetComponents<Rigidbody2D>(_rigidbodies2D);

        GetComponentsInChildren<Animator>(true, _animations);
        GetComponents<Animator>(_animations);
        
        _componentsToPause.AddRange(_components);
        _componentsToPause.AddRange(_rigidbodies);
        _componentsToPause.AddRange(_rigidbodies2D);
        _componentsToPause.AddRange(_animations);
    }

    private void OnDestroy()
    {
        if (_pauseService != null)
        {
            _pauseService.UnregisterPausable(this);
        }
    }

    public void Pause()
    {
        foreach(Rigidbody rigidBody in _rigidbodies)
        {
            _rigidBodyKinematicStates.Add(rigidBody, rigidBody.isKinematic);
            _rigidBodyCollisionStates.Add(rigidBody, rigidBody.detectCollisions);
            rigidBody.isKinematic = false;
            rigidBody.detectCollisions = true;
        }

        foreach(Rigidbody2D rigidBody in _rigidbodies2D)
        {
            _rigidBodyKinematic2DStates.Add(rigidBody, rigidBody.isKinematic);
            _rigidBodySimulated2DStates.Add(rigidBody, rigidBody.simulated);
            rigidBody.isKinematic = false;
            rigidBody.simulated = false;
        }

        foreach (Animator animator in _animations)
        {
            _animationStates.Add(animator, animator.enabled);
            animator.enabled = false;
        }
        
        foreach (MonoBehaviour component in _components)
        {
            _componentStates.Add(component, component.enabled);
            component.enabled = false;
        }

        if (OnPause != null)
            OnPause();
    }

    public void Resume()
    {
        foreach (Rigidbody rigidBody in _rigidbodies)
        {
            bool isKinematic;
            bool collisions;
            _rigidBodyKinematicStates.TryGetValue(rigidBody, out isKinematic);
            _rigidBodyCollisionStates.TryGetValue(rigidBody, out collisions);
            rigidBody.isKinematic = isKinematic;
            rigidBody.detectCollisions = collisions;
        }

        foreach (Rigidbody2D rigidBody in _rigidbodies2D)
        {
            bool isKinematic;
            bool simulated;
            _rigidBodyKinematic2DStates.TryGetValue(rigidBody, out isKinematic);
            _rigidBodySimulated2DStates.TryGetValue(rigidBody, out simulated);
            rigidBody.isKinematic = isKinematic;
            rigidBody.simulated = simulated;
        }

        foreach (Animator animator in _animations)
        {
            bool enabled;
            _animationStates.TryGetValue(animator, out enabled);
            animator.enabled = enabled;
        }
        
        foreach (MonoBehaviour component in _components)
        {
            _componentStates.TryGetValue(component, out bool enabled);
            component.enabled = enabled;
        }

        _rigidBodyKinematicStates.Clear();
        _rigidBodyCollisionStates.Clear();
        _rigidBodyKinematic2DStates.Clear();
        _rigidBodySimulated2DStates.Clear();
        _animationStates.Clear();
        _componentStates.Clear();

        if (OnResume != null)
            OnResume();
    }
}
