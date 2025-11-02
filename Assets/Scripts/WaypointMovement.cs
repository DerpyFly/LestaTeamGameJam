using System.Collections.Generic;
using UnityEngine;

public class WaypointMovement : MonoBehaviour
{
    [Header("Waypoints")]
    public List<Transform> waypoints; // Список вейпоинтов
    public bool loop = true; // Зациклить движение

    [Header("Movement Settings")]
    public float movementSpeed = 5f; // Скорость движения
    public float rotationSpeed = 2f; // Скорость поворота
    public float arrivalDistance = 0.1f; // Дистанция прибытия к вейпоинту

    [Header("Debug")]
    public bool drawGizmos = true; // Рисовать гизмо

    private int currentWaypointIndex = 0;
    private bool isMoving = true;

    void Start()
    {
        // Проверяем наличие вейпоинтов
        if (waypoints == null || waypoints.Count == 0)
        {
            Debug.LogError("No waypoints assigned!");
            isMoving = false;
            return;
        }

        // Начинаем с первого вейпоинта
        if (waypoints.Count > 0)
        {
            transform.position = waypoints[0].position;
        }
    }

    void Update()
    {
        if (!isMoving || waypoints.Count == 0) return;

        MoveToWaypoint();
    }

    void MoveToWaypoint()
    {
        // Получаем текущий целевой вейпоинт
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Вычисляем направление к вейпоинту
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // Плавный поворот в направлении движения
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Движение к вейпоинту
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementSpeed * Time.deltaTime);

        // Проверяем достижение вейпоинта
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distanceToWaypoint <= arrivalDistance)
        {
            NextWaypoint();
        }
    }

    void NextWaypoint()
    {
        currentWaypointIndex++;

        // Проверяем достижение конца маршрута
        if (currentWaypointIndex >= waypoints.Count)
        {
            if (loop)
            {
                currentWaypointIndex = 0; // Начинаем заново
            }
            else
            {
                isMoving = false; // Останавливаем движение
                Debug.Log("Reached final waypoint!");
            }
        }
    }

    // Метод для добавления вейпоинта во время выполнения
    public void AddWaypoint(Transform newWaypoint)
    {
        if (waypoints == null)
            waypoints = new List<Transform>();

        waypoints.Add(newWaypoint);
    }

    // Метод для установки нового маршрута
    public void SetWaypoints(List<Transform> newWaypoints)
    {
        waypoints = newWaypoints;
        currentWaypointIndex = 0;
        isMoving = true;
    }

    // Визуализация в редакторе
    void OnDrawGizmos()
    {
        if (!drawGizmos || waypoints == null || waypoints.Count == 0) return;

        Gizmos.color = Color.blue;

        // Рисуем линии между вейпоинтами
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;

            // Точка вейпоинта
            Gizmos.DrawSphere(waypoints[i].position, 0.2f);

            // Линия к следующему вейпоинту
            if (i < waypoints.Count - 1 && waypoints[i + 1] != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
                Gizmos.color = Color.blue;
            }
            else if (loop && waypoints[0] != null)
            {
                // Линия от последнего к первому если зациклено
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
                Gizmos.color = Color.blue;
            }
        }

        // Подсвечиваем текущий целевой вейпоинт
        if (Application.isPlaying && isMoving && currentWaypointIndex < waypoints.Count && waypoints[currentWaypointIndex] != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(waypoints[currentWaypointIndex].position, 0.3f);
        }
    }
}