using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class BotThirdPersonCharater : BotBase
{
    [Header("Derived")]
    [SerializeField]private TextMeshPro healthText;
    [SerializeField]private Transform gunTransform;
    [SerializeField]private Transform personChar;
    [SerializeField]private Transform enemy;
    [SerializeField]private BulletScript bulletPrefab;
    [SerializeField]private float bulletSpeed;
    [SerializeField]private float waitTime = 2f;
    [SerializeField]private float waitTimeShoot = 2.5f;
    [SerializeField]private float distanceCanShoot = 15f;
    private bool checkSurround;
    private Vector3 randomDir;
    private int currentPointIndex = 0;
    private float waitTimer = 0f;
    private Rigidbody rb;
    private CapsuleCollider cl;

    float DistanceToEnemy()
    {
            if (enemy == null)
                return 0f;
            else
                return Vector3.Distance(transform.position, enemy.position);
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        healthText.text = health.ToString();
        cl=GetComponent<CapsuleCollider>();
        behaviorState = BehaviorState.Patrol;
        RandomDir();
    }

    private void FixedUpdate()
    {
        DebugDrawCircle(transform.position, safeDistancePatrol, Color.red);
    }

    void RandomDir()
    {
        randomDir = new Vector3(UnityEngine.Random.Range(50,100)*(UnityEngine.Random.Range(0,2)*2-1),0,UnityEngine.Random.Range(50,100)*(UnityEngine.Random.Range(0,2)*2-1));
       Invoke(nameof(RandomDir),2f);
    }
    protected override void UpdateBotState(BehaviorState newState)
    {
        if (behaviorState != newState)
            behaviorState = newState;
    }

    protected override void Patrol()
    {
        float distanceToPatrolPoint = Vector3.Distance(transform.position, patrolPoints[currentPointIndex].position) - cl.radius;

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
                Debug.LogError("check surround");
                checkSurround = true;
                CheckSurroundings();
            }
        }
        else
        {
            checkSurround = false;
            MoveForward((patrolPoints[currentPointIndex].position - transform.position).normalized);
            //transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    void MoveForward(Vector3 directionInput)
    {
        //Debug.LogError(directionInput);
        RotateTowardsDirection(directionInput);
        rb.AddForce(directionInput * moveSpeed, ForceMode.Force);
        //Debug.LogError(transform.position.normalized);
    }

    void RotateTowardsDirection(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(direction);

            var targetRotation = Quaternion.Euler(-90, toRotation.eulerAngles.y, 0);
            personChar.rotation = Quaternion.Slerp(personChar.rotation, targetRotation,
                Time.deltaTime * rotationSpeed);
        }
    }


    void CheckSurroundings()
    {
        Action raycastAction = () =>
        {
            Ray ray = new Ray(personChar.position, -personChar.up);
            RaycastHit[] hits = Physics.RaycastAll(ray, visibleDistance);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Bot"))
                {
                    UpdateBotState(BehaviorState.Chase);
                    if (enemy != hit.collider.transform)
                    {
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

                DebugRaycast(hit);
            }
        };
        Quaternion rotationTarget = Quaternion.Euler(0, -90, 0) * personChar.rotation;
        personChar.DORotateQuaternion(rotationTarget, 2f).OnUpdate(
            () =>
            {
                if (behaviorState != BehaviorState.Patrol) DOTween.KillAll(personChar);
                raycastAction.Invoke();
            }).OnComplete(() =>
        {
            Quaternion rotationTarget1 = Quaternion.Euler(0, 145, 0) * personChar.rotation;
            personChar.DORotateQuaternion(rotationTarget1, 2f).OnUpdate(() =>
            {
                if (behaviorState != BehaviorState.Patrol) DOTween.KillAll(personChar);
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

    void DebugRaycast(RaycastHit hit)
    {
        Ray ray = new Ray(personChar.position, -personChar.up);
        Color rayColor = hit.collider != null ? Color.red : Color.green;
        Debug.DrawRay(ray.origin, ray.direction * visibleDistance, rayColor);
    }

    protected override void Chase()
    {
        MoveForward((enemy.position - transform.position).normalized);
        if (DistanceToEnemy() < distanceCanShoot)
        {
            Debug.LogError("Attack");
            rb.velocity = Vector3.zero;
            UpdateBotState(BehaviorState.Attack);
        }
    }

    protected override void Flee()
    {
        if (DistanceToEnemy() < visibleDistance)
            MoveForward((enemy.position + transform.position+randomDir).normalized);
        else
        {
            rb.velocity=Vector3.zero;
        }
    }

    protected override void Attack()
    {
        Vector3 direction = (enemy.position - transform.position).normalized;
        RotateTowardsDirection(direction);
        if (DistanceToEnemy() > distanceCanShoot)
        {
            Debug.LogError("Chase");
            UpdateBotState(BehaviorState.Chase);
        }
    }
    
    private IEnumerator Shooter()
    {
        while (enemy != null)
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
        }
    }
}