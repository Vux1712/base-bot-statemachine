using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    StateManager stateManager;

    public Transform Player;
    public Transform Enemy;
    public float AttackRange;
    public float FleeRange;

    public AttackState(Transform player, Transform enemy, float attackRange, float fleeRange)
    {
        this.Player = player;
        this.Enemy = enemy;
        AttackRange = attackRange;
        FleeRange = fleeRange;
    }
    public override void ExitState( )
    {
        
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Attack", 0, 0);
        
        this.stateManager = stateManager;

    }

    public override void UpdateState( )
    {
        CheckPlayerMoveAway();
        Attack();
        CheckFlee();
    }
    public void CheckPlayerMoveAway()
    {
        Vector3 distance = Player.position - Enemy.position;
        if (distance.magnitude > AttackRange)
        {
            stateManager.ChangeState(stateManager.chaseState);
        }
    }
    public void Attack()
    {
        stateManager.transform.LookAt(Player);
    }
    public void CheckFlee()
    {
        Vector3 distance = Player.position - Enemy.position;
        if (distance.magnitude < FleeRange)
        {
            stateManager.ChangeState(stateManager.fleeState);
        }
    }

}
