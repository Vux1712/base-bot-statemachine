using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : BaseState
{
    public override void ExitState(StateManager stateManager)
    {
        stateManager.animator.SetFloat("Atk", 0);
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Move", 0, 0);
        stateManager.animator.SetFloat("Move", 0);
        stateManager.animator.SetFloat("Atk", 1);


    }

    public override void UpdateState(StateManager stateManager)
    {

    }
}
