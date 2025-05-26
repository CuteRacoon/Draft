using UnityEngine;

public class ChickenMovement : MonoBehaviour
{
    public float moveSpeed = 3f;           // Скорость передвижения
    public float stopDurationMin = 1f;    // Минимальная продолжительность остановки
    public float stopDurationMax = 3f;    // Максимальная продолжительность остановки
    public float sphereRadius = 5f;        // Радиус сферы, в которой курица может ходить
    public Transform sphereCenter;         // Центр сферы

    private Vector3 targetPosition;         // Целевая позиция для движения
    private float stopTimer = 0f;          // Таймер для отсчета времени остановки
    private bool isMoving = false;         // Состояние движения

    private void Start()
    {
        if (sphereCenter == null)
        {
            sphereCenter = transform;
            Debug.LogWarning("Sphere center for " + gameObject.name + " not assigned, using the chicken's transform as the center");
        }
        SetNewTargetPosition();
    }

    private void Update()
    {
        if (isMoving)
        {
            // Двигаем курицу к целевой позиции
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // Проверяем, достигли ли цели
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                stopTimer = Random.Range(stopDurationMin, stopDurationMax);
            }
        }
        else
        {
            // Уменьшаем таймер остановки
            stopTimer -= Time.deltaTime;

            // Если таймер остановки закончился, начинаем движение в новом направлении
            if (stopTimer <= 0f)
            {
                SetNewTargetPosition();
                isMoving = true;
            }
        }
    }


    private void SetNewTargetPosition()
    {
        // Генерируем случайную позицию в пределах сферы
        Vector2 randomCirclePoint = Random.insideUnitCircle * sphereRadius;
        targetPosition = sphereCenter.position + new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y);

        // Устанавливаем поворот, чтобы курица смотрела в направлении движения
        transform.LookAt(targetPosition);
    }
    private void OnDrawGizmosSelected()
    {
        if (sphereCenter == null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, sphereRadius);
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(sphereCenter.position, sphereRadius);
    }
}