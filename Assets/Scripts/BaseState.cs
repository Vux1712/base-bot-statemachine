using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseState 
{
    public abstract void EnterState(StateManager stateManager);
    public abstract void UpdateState( );
    public abstract void ExitState();
}
