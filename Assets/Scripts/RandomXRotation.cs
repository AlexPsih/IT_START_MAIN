using UnityEngine;

[RequireComponent(typeof(EnemyFollow))]
public class RandomXRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 90f; // Скорость вращения в градусах в секунду
    private float minXAngle = -135f; // Минимальный угол по X
    private float maxXAngle = 25f;   // Максимальный угол по X

    private EnemyFollow enemyFollow;
    private bool isRotating = false;
    private float targetXAngle;
    private Quaternion targetRotation;

    void Start()
    {
        enemyFollow = GetComponent<EnemyFollow>();
        GenerateNewTargetAngle();
    }

    void Update()
    {
        // Проверяем, находится ли NPC в дружелюбном режиме и следует ли за игроком
        if (enemyFollow.aggression == EnemyFollow.AggressionType.Friendly && enemyFollow.IsFollowing())
        {
            if (!isRotating)
            {
                isRotating = true;
            }

            RotateToTargetAngle();
        }
        else if (isRotating)
        {
            isRotating = false;
        }
    }

    void RotateToTargetAngle()
    {
        // Текущая локальная ротация
        Vector3 currentEuler = transform.localEulerAngles;
        // Целевая ротация только по X, сохраняем Y и Z без изменений
        Quaternion currentRotation = Quaternion.Euler(currentEuler);
        targetRotation = Quaternion.Euler(targetXAngle, currentEuler.y, currentEuler.z);

        // Плавное вращение к целевой ротации
        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            targetRotation,
            rotationSpeed * Time.deltaTime / 180f
        );

        // Проверяем, близко ли текущее вращение к целевому (в пределах 1 градуса)
        if (Quaternion.Angle(transform.localRotation, targetRotation) < 1f)
        {
            GenerateNewTargetAngle();
        }
    }

    void GenerateNewTargetAngle()
    {
        targetXAngle = Random.Range(minXAngle, maxXAngle);
    }

    // Метод для доступа к isFollowing из EnemyFollow (добавим в EnemyFollow ниже)
    public bool IsFollowing()
    {
        return enemyFollow.IsFollowing();
    }
}