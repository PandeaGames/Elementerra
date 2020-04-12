using PandeaGames;
using Terra.MonoViews;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class FollowPlayerAIMonoView : AbstractTerraMonoComponent
    {
        private enum State
        {
            Follow,
            Idle
        }

        private State _state;
        private PlayerStateViewModel _playerStateViewModel;
        private TerraViewModel _terraViewModel;

        [SerializeField]
        private float _moveSpeed = 0.1f;
        
        [SerializeField]
        private Animator _animator;
        
        [SerializeField, Range(0, 1)]
        private float _chanceToLootAtPlayerEachFrame = 0.1f;
        
        [SerializeField]
        private float _distanceFromPlayer = 1;
        
        [SerializeField]
        private float _randomDistanceFromPlayer = 2;
        
        [SerializeField]
        private float _distanceTrench = 0.1f;

        [SerializeField] private Rigidbody _rb;

        private float _finalDistanceFromPlayer;

        protected override void Start()
        {
            _terraViewModel = Game.Instance.GetViewModel<TerraViewModel>(0);
            _playerStateViewModel = Game.Instance.GetViewModel<PlayerStateViewModel>(0);
            base.Start();
        }

        protected override void Initialize(RuntimeTerraEntity Entity)
        {
            base.Initialize(Entity);
            Random.InitState(Entity.InstanceId);
            _finalDistanceFromPlayer =
                Random.Range(_distanceFromPlayer, _distanceFromPlayer + _randomDistanceFromPlayer);
        }

        private void Update()
        {
            if (Initialized)
            {
                switch (_state)
                {
                    case State.Follow:
                    {
                        Update_Follow();
                        break;
                    }
                    case State.Idle:
                    {
                        Update_Idle();
                        break;
                    }
                }
            }
        }

        private void Update_Follow()
        {
            if (_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
            {
                _animator.Play("Walk WO Root Motion");
            }
            
            if (_terraViewModel.PlayerEntity.transform != null)
            {
                Transform playerTransform = _terraViewModel.PlayerEntity.transform;
                bool shouldLookAtPlayer = Random.Range(0, 100 - (100 * _chanceToLootAtPlayerEachFrame)) < 1;
                if (shouldLookAtPlayer)
                {
                    transform.LookAt(playerTransform);
                }
                
                _rb.AddForce(transform.forward * _moveSpeed * Time.deltaTime, ForceMode.Impulse);

                float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distanceFromPlayer + _distanceTrench < _finalDistanceFromPlayer)
                {
                    _state = State.Idle;
                    _animator.Play("Idle");
                }
            }
        }

        private void Update_Idle()
        {
            if (_terraViewModel.PlayerEntity.transform != null)
            {
                Transform playerTransform = _terraViewModel.PlayerEntity.transform;
                transform.LookAt(playerTransform);
                float distanceFromPlayer = Vector3.Distance(transform.position, playerTransform.position);

                if (distanceFromPlayer - _distanceTrench > _finalDistanceFromPlayer)
                {
                    _state = State.Follow;
                    _animator.Play("Walk WO Root Motion");
                }
            }
        }
    }
}