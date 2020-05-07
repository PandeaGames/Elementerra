using System;
using PandeaGames;
using System.Linq;
using Terra.MonoViews.Utility;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terra.MonoViews.AI
{
    public class FollowPlayerAIMonoView : AbstractTerraMonoComponent
    {
        [Flags]
        private enum State
        {
            Roaming = 0,
            Follow = 1,
            Idle = 2, 
            ProjectileAttack = 3,
            RoamingProjectileAttack = 4
        }

        private State _state;
        private PlayerStateViewModel _playerStateViewModel;
        private PlayerEntitySlaveViewModel _playerEntitySlaveViewModel;
        private TerraViewModel _terraViewModel;
        private TerraEntityMonoView _attacking;
        private float _projectileStateStart;
        private bool _hasShotProjectile;

        [SerializeField] 
        private GameObject _projectilePrefab;

        [SerializeField] private Transform _projectileSpawnPoint;
        
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

        [SerializeField] private float _attackAnimationTriggerSeconds = 1;
        [SerializeField] private float _attackAnimationCompleteTriggerSeconds = 1;

        [SerializeField] private Rigidbody _rb;
        [SerializeField] private TerraEntityColliderMonoView _terraEntityColliderMonoView;

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
            _playerEntitySlaveViewModel = Game.Instance.GetViewModel<PlayerEntitySlaveViewModel>(0);
            _playerEntitySlaveViewModel.OnSlaveChange += SlaveChange;
            Random.InitState(Entity.InstanceId);
            _finalDistanceFromPlayer =
                Random.Range(_distanceFromPlayer, _distanceFromPlayer + _randomDistanceFromPlayer);
            _state = (State) Entity.TerraLivingEntity.State;

            if (_state == State.Idle || _state == State.ProjectileAttack || _state == State.Follow)
            {
                _playerEntitySlaveViewModel.SetSlave(Entity);
            }
        }

        private void SlaveChange(RuntimeTerraEntity current, RuntimeTerraEntity updated)
        {
            if (current == Entity)
            {
                _state = State.Roaming;
            }
            else if(updated == Entity)
            {
                _state = State.Idle;
            }
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
                    case State.ProjectileAttack:
                    {
                        Update_ProjectileAttack();
                        break;
                    }
                    case State.Roaming:
                    {
                        Update_Roaming();
                        break;
                    }
                    case State.RoamingProjectileAttack:
                    {
                        Update_RoamingProjectileAttack();
                        break;
                    }
                }
                
                Entity.TerraLivingEntity.State = (int) _state;
            }
        }

        private void Update_Follow()
        {
            if (!CheckForAttackStateTransition())
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
        }

        private void Update_Roaming()
        {
            if (!CheckForAttackStateTransition())
            {
                
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

        private void Update_RoamingProjectileAttack()
        {
            UpdateAttacking(State.Roaming);
        }

        private void Update_ProjectileAttack()
        {
            UpdateAttacking(State.Idle);
        }

        private void UpdateAttacking(State stateAfterAttackComplete)
        {
            if (_attacking == null)
            {
                _state = stateAfterAttackComplete;
                return;
            }
            
            transform.LookAt(_attacking.transform);
            if (!_hasShotProjectile && _projectileStateStart + _attackAnimationTriggerSeconds < Time.time)
            {
                //Shoot Projectile
                _hasShotProjectile = true;
                TerraProjectile terraProjectile  = Instantiate(_projectilePrefab, _projectileSpawnPoint.position,
                    transform.rotation, transform.parent).GetComponent<TerraProjectile>();
                terraProjectile.SetAttackDef(new AttackDef(
                    Entity.EntityTypeData.AttackDamage,
                    (transform.rotation * Vector3.forward),
                    Entity
                ));
            }

            if (_projectileStateStart + _attackAnimationCompleteTriggerSeconds < Time.time)
            {
                _state = stateAfterAttackComplete;
                _animator.Play("Idle");
            }
        }

        private bool CheckForAttackStateTransition()
        {
            foreach (TerraEntityMonoView entityMonoView in _terraEntityColliderMonoView.CollidingWith)
            {
                if (string.IsNullOrEmpty(Entity.EntityTypeData.AggroLabel))
                {
                    continue;
                }
                
                if (entityMonoView.Entity.EntityTypeData.Labels.Contains(Entity.EntityTypeData.AggroLabel))
                {
                    Attack(entityMonoView, State.ProjectileAttack);
                    return true;
                }
            }

            return false;
        }

        private void Attack(TerraEntityMonoView entityMonoView, State attackState)
        {
            _projectileStateStart = Time.time;
            _hasShotProjectile = false;
            _attacking = entityMonoView;
            _state = attackState;
            _animator.Play("Projectile Attack");
        }
    }
}