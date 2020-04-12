using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Terra.MonoViews;
using Terra.MonoViews.Utility;
using Terra.SerializedData.Entities;
using UnityEngine;

public class HostileAIMonoView : AbstractTerraMonoComponent
{
    private enum State
    {
        Tracking,
        Idle,
        Attack
    }
    
    [SerializeField]
    private TerraEntityColliderMonoView _terraEntityColliderMonoView;

    [SerializeField]
    private Transform _attackColliderTransform;
    
    [SerializeField]
    private Animator _animator;
    
    private State _state;
    private TerraEntityMonoView _attacking;
    
    // Start is called before the first frame update
    protected override void Initialize(RuntimeTerraEntity Entity)
    {
        base.Initialize(Entity);
        
        _attackColliderTransform.localScale = new Vector3(
            Entity.EntityTypeData.AttackRange,
            Entity.EntityTypeData.AttackRange,
            Entity.EntityTypeData.AttackRange
            );

        _state = State.Idle;
    }

    private void Update()
    {
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
            }
        }
    }

    private void Update_Tracking()
    {
        
    }
    
    private void Update_Idle()
    {
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
       }
    }
}
