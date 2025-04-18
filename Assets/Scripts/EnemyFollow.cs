using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class EnemyFollow : MonoBehaviour
{
    public enum BehaviorType { PatrolRadius, PatrolPoints }
    public enum AggressionType { Passive, Aggressive, Friendly }

    [Header("General Settings")]
    public float speed = 5f;
    public float rotationSpeed = 5f;

    [Header("Aggression Settings")]
    public AggressionType aggression = AggressionType.Passive;
    public Transform target;
    public float triggerRadius = 10f;

    [Header("Friendly Settings")]
    public float friendlyTriggerRadius = 7f;
    public float friendlyFollowTime = 3f;
    public float friendlyCooldownTime = 5f;
    public float friendlyFollowDistance = 3f;
    [SerializeField] private AudioClip[] friendlyAudioClips = new AudioClip[0];
    [SerializeField] private Transform armTransform; // Ссылка на Transform руки
    public float armRotationSpeed = 90f; // Скорость вращения руки
    public float armMinXAngle = -135f;  // Минимальный угол по X для руки
    public float armMaxXAngle = 25f;    // Максимальный угол по X для руки

    [Header("Patrol Settings")]
    public BehaviorType behavior = BehaviorType.PatrolRadius;
    public float patrolRadius = 5f;
    public float patrolChangeTime = 3f;
    [SerializeField] private Transform[] patrolPoints = new Transform[2];

    private Rigidbody rb;
    private AudioSource audioSource;
    private bool isFollowing = false;
    private bool isFriendlyCooldown = false;
    private Vector3 startPosition;
    private Vector3 patrolTarget;
    private float patrolTimer;
    private int currentPointIndex = 0;
    private Quaternion armInitialRotation; // Изначальная ротация руки
    private float armTargetXAngle; // Целевой угол для руки
    private Quaternion armTargetRotation; // Целевая ротация для руки

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        audioSource = GetComponent<AudioSource>();
        startPosition = transform.position;

        // Сохраняем начальную ротацию руки, если она задана
        if (armTransform != null)
        {
            armInitialRotation = armTransform.localRotation;
            GenerateNewArmTargetAngle();
        }
        else if (aggression == AggressionType.Friendly)
        {
            Debug.LogWarning("Arm Transform is not assigned for Friendly mode!", this);
        }

        if (behavior == BehaviorType.PatrolRadius)
            GenerateNewPatrolPoint();
        else if (patrolPoints.Length > 0 && patrolPoints[0] != null)
            patrolTarget = patrolPoints[0].position;

        if (aggression == AggressionType.Friendly && friendlyAudioClips.Length > 0 && audioSource == null)
        {
            Debug.LogWarning("AudioSource component is missing on Friendly NPC!", this);
        }
    }

    void FixedUpdate()
    {
        // Passive mode — only patrols, no interaction with target
        if (aggression == AggressionType.Passive)
        {
            if (behavior == BehaviorType.PatrolRadius)
            {
                PatrolRadius();
            }
            else if (patrolPoints.Length > 0 && patrolPoints[currentPointIndex] != null)
            {
                PatrolPoints();
            }
            return;
        }

        // Aggressive mode
        if (aggression == AggressionType.Aggressive && target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            isFollowing = distanceToTarget <= triggerRadius;

            if (isFollowing)
            {
                FollowTarget();
                return;
            }
        }

        // Friendly mode
        if (aggression == AggressionType.Friendly && target != null && !isFriendlyCooldown && !isFollowing)
        {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget <= friendlyTriggerRadius)
            {
                isFollowing = true;
                StartCoroutine(FriendlyFollowCoroutine());
            }
        }

        // Patrolling — only if not following
        if (!isFollowing)
        {
            if (behavior == BehaviorType.PatrolRadius)
            {
                PatrolRadius();
            }
            else if (patrolPoints.Length > 0 && patrolPoints[currentPointIndex] != null)
            {
                PatrolPoints();
            }
        }
    }

    void FollowTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        RotateTowards(direction);
        MoveTowards(direction);
    }

    void FriendlyFollow()
    {
        Vector3 direction = (target.position - transform.position);
        float distance = direction.magnitude;

        if (distance > friendlyFollowDistance)
        {
            direction.Normalize();
            RotateTowards(direction);
            MoveTowards(direction);
        }
    }

    void RotateTowards(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
    }

    void MoveTowards(Vector3 direction)
    {
        Vector3 targetPosition = transform.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);
    }

    void GoToNearestPatrolPoint()
    {
        if (behavior == BehaviorType.PatrolPoints && patrolPoints.Length > 0)
        {
            float minDistance = Mathf.Infinity;
            int nearestIndex = 0;

            for (int i = 0; i < patrolPoints.Length; i++)
            {
                if (patrolPoints[i] != null)
                {
                    float dist = Vector3.Distance(transform.position, patrolPoints[i].position);
                    if (dist < minDistance)
                    {
                        minDistance = dist;
                        nearestIndex = i;
                    }
                }
            }

            currentPointIndex = nearestIndex;
            if (patrolPoints[currentPointIndex] != null)
            {
                patrolTarget = patrolPoints[currentPointIndex].position;
            }
        }
        else if (behavior == BehaviorType.PatrolRadius)
        {
            GenerateNewPatrolPoint();
        }
    }

    void PatrolRadius()
    {
        Vector3 direction = (patrolTarget - transform.position).normalized;
        RotateTowards(direction);
        MoveTowards(direction);

        patrolTimer -= Time.fixedDeltaTime;
        if (Vector3.Distance(transform.position, patrolTarget) < 1f || patrolTimer <= 0)
        {
            GenerateNewPatrolPoint();
            patrolTimer = patrolChangeTime;
        }
    }

    void PatrolPoints()
    {
        if (patrolPoints.Length == 0 || patrolPoints[currentPointIndex] == null)
            return;

        Vector3 direction = (patrolTarget - transform.position).normalized;
        RotateTowards(direction);
        MoveTowards(direction);

        if (Vector3.Distance(transform.position, patrolTarget) < 1f)
        {
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
            if (patrolPoints[currentPointIndex] != null)
            {
                patrolTarget = patrolPoints[currentPointIndex].position;
            }
        }
    }

    void GenerateNewPatrolPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;
        patrolTarget = startPosition + new Vector3(randomCircle.x, 0, randomCircle.y);
    }

    IEnumerator FriendlyFollowCoroutine()
    {
        // Play random audio clip if available
        if (audioSource != null && friendlyAudioClips.Length > 0)
        {
            AudioClip clip = friendlyAudioClips[Random.Range(0, friendlyAudioClips.Length)];
            if (clip != null)
            {
                audioSource.PlayOneShot(clip);
            }
        }

        // Инициализируем вращение руки
        if (armTransform != null)
        {
            GenerateNewArmTargetAngle();
        }

        float followTimer = friendlyFollowTime;
        while (followTimer > 0)
        {
            if (target == null) break;
            FriendlyFollow();
            if (armTransform != null)
            {
                RotateArmToTargetAngle();
            }
            followTimer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        // Возвращаем руку к изначальной ротации
        if (armTransform != null)
        {
            while (Quaternion.Angle(armTransform.localRotation, armInitialRotation) > 1f)
            {
                RotateArmToInitialRotation();
                yield return new WaitForFixedUpdate();
            }
            armTransform.localRotation = armInitialRotation; // Гарантируем точное возвращение
        }

        isFollowing = false;
        GoToNearestPatrolPoint();
        yield return new WaitForSeconds(friendlyCooldownTime);
        isFriendlyCooldown = false;
    }

    void RotateArmToTargetAngle()
    {
        // Текущая локальная ротация руки
        Vector3 currentEuler = armTransform.localEulerAngles;
        // Целевая ротация только по X, сохраняем Y и Z
        armTargetRotation = Quaternion.Euler(armTargetXAngle, currentEuler.y, currentEuler.z);

        // Плавное вращение к целевой ротации
        armTransform.localRotation = Quaternion.Slerp(
            armTransform.localRotation,
            armTargetRotation,
            armRotationSpeed * Time.fixedDeltaTime / 180f
        );

        // Проверяем, близко ли текущее вращение к целевому (в пределах 1 градуса)
        if (Quaternion.Angle(armTransform.localRotation, armTargetRotation) < 1f)
        {
            GenerateNewArmTargetAngle();
        }
    }

    void RotateArmToInitialRotation()
    {
        // Плавное вращение к изначальной ротации
        armTransform.localRotation = Quaternion.Slerp(
            armTransform.localRotation,
            armInitialRotation,
            armRotationSpeed * Time.fixedDeltaTime / 180f
        );
    }

    void GenerateNewArmTargetAngle()
    {
        armTargetXAngle = Random.Range(armMinXAngle, armMaxXAngle);
    }

    // Публичный метод для проверки состояния isFollowing
    public bool IsFollowing()
    {
        return isFollowing;
    }

    void OnDrawGizmosSelected()
    {
        if (aggression == AggressionType.Aggressive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, triggerRadius);
        }
        else if (aggression == AggressionType.Friendly)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, friendlyTriggerRadius);
        }

        if (behavior == BehaviorType.PatrolRadius)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPosition, patrolRadius);
        }
        else if (patrolPoints != null && patrolPoints.Length > 0)
        {
            Gizmos.color = Color.yellow;
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
        else if (script.aggression == EnemyFollow.AggressionType.Friendly)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("target"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("friendlyTriggerRadius"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("friendlyFollowTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("friendlyCooldownTime"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("friendlyFollowDistance"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("friendlyAudioClips"), true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("armTransform"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("armRotationSpeed"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("armMinXAngle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("armMaxXAngle"));
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