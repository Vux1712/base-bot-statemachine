using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndMoveState : BaseState
{
    StateManager stateManager;
    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Move", 0);
        stateManager.animator.SetFloat("Move", 1);
        stateManager.animator.SetFloat("Atk", 1);
        this.stateManager = stateManager;
    }

    public override void ExitState( )
    {
        stateManager.animator.SetFloat("Atk", 0);
    }

    public override void UpdateState( )
    {

    }
}
