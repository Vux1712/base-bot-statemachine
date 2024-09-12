
using UnityEngine;

public class PatrolSate : BaseState
{
    StateManager stateManager;

    public Transform Player;
    public Transform Enemy;
    public WayPoint wayPoint;
    public float DetectRange;

    public int currentWayPoint = 0;
    public PatrolSate(Transform player, Transform enemy, WayPoint wayPoint, float detectRange)
    {
        this.Player = player;
        this.Enemy = enemy;
        this.wayPoint = wayPoint;
        this.DetectRange = detectRange;
    }
    public override void ExitState()
    {
            
    }

    public override void EnterState(StateManager stateManager)
    {
        stateManager.animator.CrossFade("Patrol", 0, 0);
        
        this.stateManager = stateManager;
    }

    public override void UpdateState()
    {
        DetectPlayer();
        MoveFolowPath();
    }

    public void DetectPlayer()
    {
        Vector3 distence = Player.position - Enemy.position;
        if(distence.magnitude < DetectRange)
        {
            stateManager.ChangeState(stateManager.chaseState);
        }
    }
    public void MoveFolowPath()
    {
        stateManager.transform.position = Vector3.MoveTowards(stateManager.transform.position, wayPoint.points[currentWayPoint].transform.position, Time.deltaTime);
        if(stateManager.transform.position == wayPoint.points[currentWayPoint].transform.position)
        {
            currentWayPoint++;
            if(currentWayPoint >= wayPoint.points.Count)
            {
                currentWayPoint = 0;
            }
        }
    }

}
