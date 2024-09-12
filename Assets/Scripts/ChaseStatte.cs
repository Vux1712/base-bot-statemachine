
using UnityEngine;

public class ChaseState : BaseState
{
    StateManager stateManager;

    public Transform Player;
    public Transform Enemy;
    public float AttackRange;
    public float DetecRange;

    public ChaseState(Transform player, Transform enemy, float attackRange,float detectRange)
    {
        this.Player = player;
        this.Enemy = enemy;
        AttackRange = attackRange;
        DetecRange = detectRange;
    }
    public override void ExitState( )
    {
        
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Chase",0,0);
        this.stateManager = stateManager;
    }

    public override void UpdateState( )
    {
        CheckInAttackRange();
        MoveToPlayer();
        CheckPlayerOutDetecRange();
    }
    public void CheckInAttackRange()
    {
        Vector3 distence = Player.position - Enemy.position;
        if (distence.magnitude < AttackRange)
        {
            stateManager.ChangeState(stateManager.attackState);
        }
    }
    public void MoveToPlayer()
    {
        stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, Player.position, Time.deltaTime*2);
        stateManager.transform.LookAt(Player);
    }
    public void CheckPlayerOutDetecRange()
    {
        Vector3 distence = Player.position - Enemy.position;
        if (distence.magnitude > DetecRange)
        {
            stateManager.ChangeState(stateManager.patrolSate);
        }
    }
}
