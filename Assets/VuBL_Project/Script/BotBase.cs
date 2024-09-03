using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class BotBase : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected int health = 5;
    [SerializeField] protected float visibleDistance = 10f;
    [SerializeField] protected float safeDistancePatrol = 5f;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float rotationSpeed = 3f;
    [SerializeField] protected BehaviorState behaviorState;
    protected Rigidbody rb;
    protected CapsuleCollider cl;
    protected Animator _animator;
    protected abstract void UpdateBotState(BehaviorState newState);

    protected abstract void Patrol(); //tuan tra

    protected abstract void Chase(); //theo duoi

    protected abstract void Flee(); // chay tron

    protected abstract void Attack(); // tancong
    

    protected virtual void Start()
    {
    }

}

public enum BehaviorState
{
    Patrol,
    Chase,
    Attack,
    Flee
}