using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currenState;
    public Transform Player;
    public AttackState attackState;
    public PatrolSate patrolSate;
    public ChaseState chaseState;
    public FleeState fleeState;
    public AttackAndMoveState attackAndMoveState;

    public WayPoint wayPoint;

    public Animator animator;

    public float FleeRange = 1;
    public float AttackRange = 5;

    public float DetecRange = 10;
    // Start is called before the first frame update
    void Start()
    {
        attackState = new AttackState(Player,transform,AttackRange,FleeRange);
        patrolSate = new PatrolSate(Player,transform,wayPoint,DetecRange);
        chaseState = new ChaseState(Player,transform,AttackRange,DetecRange);
        fleeState = new FleeState(Player,transform,FleeRange,DetecRange);
        attackAndMoveState = new AttackAndMoveState();

        animator = GetComponent<Animator>();
        ChangeState(patrolSate);
    }

    // Update is called once per frame
    void Update()
    {
       currenState.UpdateState();
    }
    public void ChangeState(BaseState state)
    {
        if (currenState != null)
        {
            currenState.ExitState();
        }
        currenState = state;
        currenState.EnterState(this);
    }
    public void OnDrawGizmosSelected()
    {
        
        
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, DetecRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, AttackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, FleeRange);
    }
}
