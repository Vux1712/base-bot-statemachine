
using UnityEngine;

public class ChaseStatte : BaseState
{
    public override void ExitState(StateManager stateManager)
    {
        
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Chase",0,0);
    }

    public override void UpdateState(StateManager stateManager)
    {
        
    }
}
