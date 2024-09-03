using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    BaseState currenState;

    public AttackState attackState = new AttackState();
    public PatrolSate patrolSate = new PatrolSate();
    public ChaseStatte chaseStatte = new ChaseStatte();
    public FleeState fleeState = new FleeState();
    public AttackAndMoveState attackAndMoveState = new AttackAndMoveState();


    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        currenState = patrolSate;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") > 0)
        {
            ChangeState(patrolSate);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            ChangeState(patrolSate);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeState(chaseStatte);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            ChangeState(fleeState);
        }
        if (Input.GetKeyDown(KeyCode.K) && Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            ChangeState(attackAndMoveState);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            ChangeState(attackState);
        }
    }
    public void ChangeState(BaseState state)
    {
        if (currenState != null)
        {
            currenState.ExitState(this);
        }
        currenState = state;
        currenState.EnterState(this);
    }
}
