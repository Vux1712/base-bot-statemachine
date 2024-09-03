using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class BotThirdPersonCharater : BotBase
{
    [Header("Derived")] [SerializeField] private TextMeshPro healthText;
    [SerializeField] protected Transform[] patrolPoints;
    [SerializeField] private Transform gunTransform;
    [SerializeField] private Transform enemy;
    [SerializeField] private BulletScript bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float waitTime = 2f;
    [SerializeField] private float waitTimeShoot = 2.5f;
    [SerializeField] private float distanceCanShoot = 15f;
    public LayerMask layerMask;
    private bool checkSurround;
    private Vector3 randomDir;
    private int currentPointIndex = 0;
    private float waitTimer = 0f;

    private bool isAttack;

    float DistanceToEnemy()
    {
        if (enemy == null)
            return 0f;
        else
            return Vector3.Distance(transform.position, enemy.position);
    }

    protected override void Start()
    {
        rb = GetComponent<Rigidbody>();
        cl = GetComponent<CapsuleCollider>();
        _animator = GetComponent<Animator>();
        healthText.text = health.ToString();
        behaviorState = BehaviorState.Patrol;
        RandomDir();
    }

    void Update()
    {
        DebugDrawCircle(transform.position, safeDistancePatrol, Color.red);
        Debug.DrawRay(transform.position, transform.forward * visibleDistance, Color.green);

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

    private void FixedUpdate()
    {
    }

    void RandomDir()
    {
        randomDir = new Vector3(UnityEngine.Random.Range(50, 100) * (UnityEngine.Random.Range(0, 2) * 2 - 1), 0,
            UnityEngine.Random.Range(50, 100) * (UnityEngine.Random.Range(0, 2) * 2 - 1));
        Invoke(nameof(RandomDir), 2f);
    }

    protected override void UpdateBotState(BehaviorState newState)
    {
        if (behaviorState != newState)
            behaviorState = newState;
    }

    protected override void Patrol()
    {
        float distanceToPatrolPoint =
            Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) - cl.radius;
        if (distanceToPatrolPoint < safeDistancePatrol)
        {
            waitTimer += Time.deltaTime;
            rb.velocity = Vector3.zero;
            if (waitTimer >= waitTime)
            {
                currentPointIndex =
                    (currentPointIndex + 1) % patrolPoints.Length /* UnityEngine.Random.Range(0,patrolPoints.Length)*/;
                waitTimer = 0f;
            }

            if (!checkSurround)
            {
                checkSurround = true;
                _animator.SetFloat("Speed", 0);
                CheckAheadPatrol();
            }
        }
        else
        {
            checkSurround = false;
            MoveForward((patrolPoints[currentPointIndex].position - transform.position).normalized);
        }
    }

    protected override void Chase()
    {
        MoveForward((enemy.position - transform.position).normalized);
        if (DistanceToEnemy() < distanceCanShoot && !isAttack)
        {
            isAttack = true;
            Debug.LogError("Attack");
            rb.velocity = Vector3.zero;
            UpdateBotState(BehaviorState.Attack);
        }
    }

    protected override void Flee()
    {
        /*Collider[] hitColliders = Physics.OverlapSphere(transform.position , visibleDistance+10, layerMask);

        bool isBlocked = false;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Blocking Wall") && behaviorState == BehaviorState.Flee)
            {
                MoveForward((- transform.forward ).normalized);
                isBlocked = true;
            }
        }

        if (isBlocked) return;*/
        if (DistanceToEnemy() < visibleDistance)
        {
            MoveForward((enemy.position + transform.position + randomDir).normalized);
        }
        else
        {
            _animator.SetFloat("Speed", 0);
            rb.velocity = Vector3.zero;
        }
    }

    protected override void Attack()
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        RotateTowardsDirection(direction);
        if (DistanceToEnemy() > distanceCanShoot && isAttack)
        {
            isAttack = false;
            Debug.LogError("Chase");
            _animator.SetBool("fire", false);
            UpdateBotState(BehaviorState.Chase);
        }
        else if (!_animator.GetBool("fire"))
        {
            _animator.SetBool("fire", true);
            // StartCoroutine(Shooter());
        }
    }

    void MoveForward(Vector3 directionInput)
    {
        RotateTowardsDirection(directionInput);
        _animator.SetFloat("Speed", moveSpeed);
        rb.AddForce(directionInput * moveSpeed, ForceMode.Force);
        //Debug.LogError(transform.position.normalized);
    }

    void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);

            var targetRotation = Quaternion.Euler(0, toRotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation,
                Time.deltaTime * rotationSpeed);
        }
    }


    void CheckAheadPatrol()
    {
        Action raycastAction = () =>
        {
            RaycastHit[] hits;
            Ray ray = new Ray(transform.position, transform.forward);
            hits=Physics.RaycastAll(ray, visibleDistance);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Bot"))
                {
                    UpdateBotState(BehaviorState.Chase);
                    if (enemy != hit.collider.transform)
                    {
                        moveSpeed += 0.5f;
                        enemy = hit.collider.transform;
                        StartCoroutine(Shooter());
                        break;
                    }
                }

                if (hit.collider.CompareTag("Player"))
                {
                    if (enemy == null)
                    {
                        moveSpeed += 1f;
                        enemy = hit.collider.transform;
                        break;
                    }
                }

                if (hit.collider.CompareTag("Blocking Wall"))
                {
                    //Is Blocked by wall => add new direction
                }
            }
        };
        Quaternion rotationTarget = Quaternion.Euler(0, -90, 0) * transform.rotation;
        transform.DORotateQuaternion(rotationTarget, 2f).OnUpdate(
            () =>
            {
                if (behaviorState != BehaviorState.Patrol) DOTween.KillAll(transform);
                raycastAction.Invoke();
            }).OnComplete(() =>
        {
            Quaternion rotationTarget1 = Quaternion.Euler(0, 145, 0) * transform.rotation;
            transform.DORotateQuaternion(rotationTarget1, 2f).OnUpdate(() =>
            {
                if (behaviorState != BehaviorState.Patrol) DOTween.KillAll(transform);
                raycastAction.Invoke();
            });
        });
    }

    void DebugDrawCircle(Vector3 center, float radius, Color color)
    {
        float theta = 0f;
        float x = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta);
        Vector3 pos = center + new Vector3(x, 0, z);
        Vector3 lastPos = pos;

        for (theta = 0.1f; theta < Mathf.PI * 2; theta += 0.1f)
        {
            x = radius * Mathf.Cos(theta);
            z = radius * Mathf.Sin(theta);
            Vector3 newPos = center + new Vector3(x, 0, z);
            Debug.DrawLine(lastPos, newPos, color);
            lastPos = newPos;
        }

        Debug.DrawLine(lastPos, center + new Vector3(radius, 0, 0), color);
    }

    private IEnumerator Shooter()
    {
        while (enemy != null /*&& _animator.GetBool("fire")*/)
        {
            var bullet = Instantiate(bulletPrefab);
            bullet.transform.position = this.gunTransform.position;
            bullet.ShootWhenActive(enemy.position, bulletSpeed);
            yield return new WaitForSeconds(waitTimeShoot);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            health--;
            healthText.text = health.ToString();
            UpdateBotState(BehaviorState.Flee);
            //health=0 => die
        }
    }
}