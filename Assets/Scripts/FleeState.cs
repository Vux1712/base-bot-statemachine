
using UnityEngine;

public class FleeState : BaseState
{
    public override void ExitState(StateManager stateManager)
    {
       
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Flee",0,0);
    }

    public override void UpdateState(StateManager stateManager)
    {
        
    }
}
