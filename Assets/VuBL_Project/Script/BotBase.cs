using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BotBase : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] protected int health = 5;
    [SerializeField] protected float visibleDistance = 10f;
    [SerializeField] protected float safeDistancePatrol = 5f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 3f;
    [SerializeField] protected BehaviorState behaviorState;

    protected abstract void UpdateBotState(BehaviorState newState);

    protected abstract void Patrol(); //tuan tra

    protected abstract void Chase(); //theo duoi

    protected abstract void Flee(); // chay tron

    protected abstract void Attack(); // tancong
    

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        //UpdateBotState();
        switch (behaviorState)
        {
            case BehaviorState.Patrol:
                Patrol();
                break;
            case BehaviorState.Chase:
                Chase();
                break;
            case BehaviorState.Attack:
                Attack();
                break;
            case BehaviorState.Flee:
                Flee();
                break;
        }
    }
}

public enum BehaviorState
{
    Patrol,
    Chase,
    Attack,
    Flee
}