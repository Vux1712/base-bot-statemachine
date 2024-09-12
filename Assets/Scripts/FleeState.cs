
using UnityEngine;

public class FleeState : BaseState
{
    StateManager stateManager;
    public Transform Player;
    public Transform Enemy;
    public float FleeRange;
    public float DetectRange;

    public FleeState(Transform player, Transform enemy, float flleeRange, float detectRange)
    {
        this.Player = player;
        this.Enemy = enemy;
        FleeRange = flleeRange;
        DetectRange = detectRange;
    }
    public override void ExitState( )
    {
       
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Flee",0,0);
        this.stateManager = stateManager;
        
    }

    public override void UpdateState( )
    {
        OnPlayerInFleeRange();
        MoveAwayFromPlayer();
        CheckPlayerOutFleeRange();
    }
    public void OnPlayerInFleeRange()
    {
        Vector3 distance = Player.position - Enemy.position;
        if (distance.magnitude < FleeRange)
        {
            stateManager.ChangeState(stateManager.fleeState);
        }
    }
    public void MoveAwayFromPlayer()
    {
        stateManager.transform.Translate(Vector3.back * Time.deltaTime);
    }
    public void CheckPlayerOutFleeRange()
    {
        Vector3 distance = Player.position - Enemy.position;
        if (distance.magnitude >= DetectRange)
        {
            stateManager.ChangeState(stateManager.patrolSate);
        }
    }
}
