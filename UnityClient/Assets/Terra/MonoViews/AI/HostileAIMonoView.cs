using System.Linq;
using PandeaGames;
using Terra.MonoViews.Utility;
using Terra.SerializedData.Entities;
using Terra.ViewModels;
using UnityEngine;

namespace Terra.MonoViews.AI
{
    public class HostileAIMonoView : AbstractTerraMonoComponent
    {
        private enum State
        {
            Tracking,
            Idle,
            Attack,
            Death
        }

        [SerializeField] private TerraEntityColliderMonoView _terraEntityColliderMonoView;
        [SerializeField] private TerraEntityColliderMonoView _attackEntityCollider;
        [SerializeField] private float _manuelTriggerEventTime;
        [SerializeField] private float _manuelDeathTriggerEventTimeSeconds;
        [SerializeField] private float _attackForceMagnitude = 2;
        [SerializeField] private float _attackForceVertical = 5;

        [SerializeField] private Animator _animator;

        private State _state;
        private TerraEntityMonoView _attacking;
        private float _startOfAttackPhase;
        private bool _hasAttacked;
        private float _deathStartTime;

        // Start is called before the first frame update
        protected override void Initialize(RuntimeTerraEntity Entity)
        {
            base.Initialize(Entity);
            _state = State.Idle;
        }

        private void Update()
        {
            if (_state != State.Death && Entity.IsDead())
            {
                _deathStartTime = Time.time;
                _state = State.Death;
                _animator.Play("Die");
            }
            
            if (Initialized)
            {
                switch (_state)
                {
                    case State.Tracking:
                    {
                        Update_Tracking();
                        break;
                    }
                    case State.Idle:
                    {
                        Update_Idle();
                        break;
                    }
                    case State.Attack:
                    {
                        Update_Attack();
                        break;
                    }
                    case State.Death:
                    {
                        Update_Death();
                        break;
                    }
                }
            }
        }

        private void Update_Death()
        {
            if (Time.time > _manuelDeathTriggerEventTimeSeconds + _deathStartTime)
            {
                Entity.ExpireEntity();
            }
        }
        
        private void Update_Tracking()
        {

        }

        private void Update_Idle()
        {
            _startOfAttackPhase = Time.time;
            _hasAttacked = false;
            
            foreach (TerraEntityMonoView entityMonoView in _terraEntityColliderMonoView.CollidingWith)
            {
                if (entityMonoView.Entity.EntityTypeData.Labels.Contains(Entity.EntityTypeData.AggroLabel))
                {
                    _attacking = entityMonoView;
                    _state = State.Attack;
                    _animator.Play("Bite");
                }
            }
        }

        private void Update_Attack()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || _attacking == null)
            {
                _state = State.Idle;
            }
            else
            {
                transform.LookAt(_attacking.transform);

                if (!_hasAttacked && Time.time > _startOfAttackPhase + _manuelTriggerEventTime)
                {
                    Vector3 force = (transform.rotation * Vector3.forward) * _attackForceMagnitude;
                    force.y = _attackForceVertical;
                    foreach (TerraEntityMonoView entityMonoView in _attackEntityCollider.CollidingWith)
                    {
                        if (entityMonoView.Entity.EntityTypeData.Labels.Contains(Entity.EntityTypeData.AggroLabel))
                        {
                            entityMonoView.Attack(new AttackDef(
                                Entity.EntityTypeData.AttackDamage,
                                (transform.rotation * Vector3.forward) * _attackForceMagnitude,
                                Entity
                                ));
                        }
                    }
                    
                    _hasAttacked = true;
                }
            }
        }
    }
}