
using UnityEngine;

public class PatrolSate : BaseState
{
    public override void ExitState(StateManager stateManager)
    {
        //stateManager.animator.SetFloat("Move",0);
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Move", 0, 0);
        stateManager.animator.SetFloat("Move", 1);

    }

    public override void UpdateState(StateManager stateManager)
    {

    }
}
