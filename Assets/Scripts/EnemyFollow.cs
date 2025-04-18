using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
public class EnemyFollow : MonoBehaviour
{
    public enum BehaviorType
    {
        PatrolRadius,
        PatrolPoints
    }

    public enum AggressionType
    {
        Passive,  // Не преследует
        Aggressive // Преследует цель в радиусе
    }

    [Header("General Settings")]
    public float speed = 5f;
    public float rotationSpeed = 5f;

    [Header("Aggression Settings")]
    public AggressionType aggression = AggressionType.Passive;
    public Transform target; // Цель для агрессивного поведения (заменил player на target)
    public float triggerRadius = 10f; // Радиус преследования для агрессивного режима

    [Header("Patrol Settings")]
    public BehaviorType behavior = BehaviorType.PatrolRadius;

    [Header("Radius Patrol Settings")]
    public float patrolRadius = 5f;
    public float patrolChangeTime = 3f;

    [Header("Points Patrol Settings")]
    [SerializeField] private Transform[] patrolPoints = new Transform[2];

    private Rigidbody rb;
    private bool isFollowing = false;
    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float patrolTimer;
    private int currentPointIndex = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        startPosition = transform.position;

        if (behavior == BehaviorType.PatrolRadius)
        {
            GenerateNewPatrolPoint();
        }
        else if (patrolPoints.Length > 0 && patrolPoints[0] != null)
        {
            patrolTarget = patrolPoints[0].position;
        }
    }

    void FixedUpdate()
    {
        // Проверяем агрессивное поведение
        if (aggression == AggressionType.Aggressive && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            isFollowing = distanceToTarget <= triggerRadius;

            if (isFollowing)
            {
                FollowTarget();
                return; // Прерываем, чтобы не патрулировать во время преследования
            }
        }

        // Если не преследуем или пассивный режим — патрулируем
        if (behavior == BehaviorType.PatrolRadius)
        {
            PatrolRadius();
        }
        else if (patrolPoints.Length > 0 && patrolPoints[currentPointIndex] != null)
        {
            PatrolPoints();
        }
    }

    void FollowTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        Vector3 targetPosition = transform.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    void PatrolRadius()
    {
        Vector3 direction = (patrolTarget - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        Vector3 targetPosition = transform.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        patrolTimer -= Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, patrolTarget) < 1f || patrolTimer <= 0)
        {
            GenerateNewPatrolPoint();
            patrolTimer = patrolChangeTime;
        }
    }

    void PatrolPoints()
    {
        Vector3 direction = (patrolTarget - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        Vector3 targetPosition = transform.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            patrolTarget = patrolPoints[currentPointIndex].position;
        }
    }

    void GenerateNewPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    void OnDrawGizmosSelected()
    {
        if (aggression == AggressionType.Aggressive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }

        if (behavior == BehaviorType.PatrolRadius)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPosition, patrolRadius);
        }
        else if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.3f);
                    if (i > 0 && patrolPoints[i - 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i - 1].position, patrolPoints[i].position);
                    }
                }
            }
            if (patrolPoints.Length > 1 && patrolPoints[0] != null && patrolPoints[patrolPoints.Length - 1] != null)
            {
                Gizmos.DrawLine(patrolPoints[patrolPoints.Length - 1].position, patrolPoints[0].position);
            }
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyFollow))]
public class EnemyFollowEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EnemyFollow script = (EnemyFollow)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("speed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationSpeed"));

        EditorGUILayout.PropertyField(serializedObject.FindProperty("aggression"));

        if (script.aggression == EnemyFollow.AggressionType.Aggressive)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("triggerRadius"));
        }

        EditorGUILayout.PropertyField(serializedObject.FindProperty("behavior"));

        if (script.behavior == EnemyFollow.BehaviorType.PatrolRadius)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolRadius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolChangeTime"));
        }
        else if (script.behavior == EnemyFollow.BehaviorType.PatrolPoints)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("patrolPoints"), true);
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif