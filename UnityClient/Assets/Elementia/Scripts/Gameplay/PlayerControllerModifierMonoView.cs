using System.Collections;
using System.Collections.Generic;
using Invector.vCharacterController;
using PandeaGames;
using Terra.ViewModels;
using UnityEngine;

public class PlayerControllerModifierMonoView : MonoBehaviour
{
    [SerializeField] private vThirdPersonController _controller;

    [SerializeField] private float _walkSpeedModifier = 1;
    [SerializeField] private float _runSpeedModifier = 1;

    private float _walkSpeed;
    
    private float _runSpeed;
    private PlayerStateViewModel _playerStateViewModel;
    
    // Start is called before the first frame update
    void Start()
    {
        _walkSpeed = _controller.freeSpeed.walkSpeed;
        _runSpeed = _controller.freeSpeed.runningSpeed;
        _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
    }

    // Update is called once per frame
    void Update()
    {
        _controller.freeSpeed.walkByDefault = _playerStateViewModel.IsHoldingItem;
        _controller.freeSpeed.walkSpeed = _playerStateViewModel.IsHoldingItem ? _walkSpeed * _walkSpeedModifier : _walkSpeed;
        _controller.freeSpeed.runningSpeed = _playerStateViewModel.IsHoldingItem ? _runSpeed * _runSpeedModifier : _runSpeed;
    }
}
