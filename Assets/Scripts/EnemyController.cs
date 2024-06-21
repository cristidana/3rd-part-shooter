using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private float patrolDistance = 30.0f;
    [SerializeField] private int numberOfPatrolPoints = 4;
    private GameManager gameManager;

    private List<Vector3> patrolPoints;
    private int currentPatrolIndex;
    private Transform player;
    private bool isChasing;
    private float currentHealth;

    private float detectionRange;
    private float attackRange;

    private Animator animator;
    private bool isMoving = true;

    private float lastAttackTime;
    private float attackCooldown = 1.0f;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        InitializeVariables();
    }

    private void Update()
    {
        UpdateState();
        CheckForPlayer();
        UpdateAnimator();
    }

    private void InitializeVariables()
    {
        attackRange = enemyData.attackRange;
        detectionRange = enemyData.detectionRange;
        currentHealth = enemyData.maxHealth;
        currentPatrolIndex = 0;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        isChasing = false;
        patrolPoints = GeneratePatrolPoints(patrolDistance, numberOfPatrolPoints);
        animator = GetComponent<Animator>();
        lastAttackTime = -attackCooldown;
    }

    private void UpdateState()
    {
        if (isChasing)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    private void Patrol()
    {
        if (patrolPoints.Count == 0) return;

        Vector3 targetPatrolPoint = patrolPoints[currentPatrolIndex];
        float distanceToPoint = Vector3.Distance(transform.position, targetPatrolPoint);
        float arrivalThreshold = 1.4f;

        if (distanceToPoint <= arrivalThreshold)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
        else
        {
            isMoving = true;
            MoveTowards(targetPatrolPoint, enemyData.patrolSpeed);
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= enemyData.maxApproachDistance)
        {
            isMoving = false;
            LookAtPlayer();
            Attack();
        }
        else
        {
            isMoving = true;
            MoveTowards(player.position, enemyData.chaseSpeed);
            Attack();
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            Gun gun = GetComponentInChildren<Gun>();
            if (gun != null)
            {
                gun.EnemyShoot();
            }

            lastAttackTime = Time.time;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + enemyData.yRotationOffset, 0);
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Euler(0, targetRotation.eulerAngles.y + enemyData.yRotationOffset, 0);

        transform.position += direction * speed * Time.deltaTime;
    }

    private void CheckForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            isChasing = true;
        }
    }

    private List<Vector3> GeneratePatrolPoints(float distance, int numberOfPoints)
    {
        List<Vector3> points = new List<Vector3>();

        Vector3 startPoint = transform.position;
        points.Add(startPoint + new Vector3(distance, 0, distance));
        points.Add(startPoint + new Vector3(-distance, 0, distance));
        points.Add(startPoint + new Vector3(-distance, 0, -distance));
        points.Add(startPoint + new Vector3(distance, 0, -distance));

        return points;
    }

    private void UpdateAnimator()
    {
        animator.SetBool("lookAt", isMoving);
    }

    private void OnDrawGizmos()
    {
        DrawGizmoDetectionRange();
        DrawGizmoAttackRange();
        DrawGizmoPatrolPoints();
    }

    private void DrawGizmoDetectionRange()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

    private void DrawGizmoAttackRange()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    private void DrawGizmoPatrolPoints()
    {
        Gizmos.color = Color.blue;
        if (patrolPoints != null && patrolPoints.Count > 0)
        {
            foreach (Vector3 point in patrolPoints)
            {
                Gizmos.DrawWireSphere(point, 0.5f);
            }
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameManager.EnemyDestroyed(this);
        Destroy(gameObject);

    }
}
