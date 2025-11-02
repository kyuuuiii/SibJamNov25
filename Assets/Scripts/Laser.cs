using UnityEngine;
using System.Collections.Generic;

public class Laser : MonoBehaviour
{
    public GameObject linePrefab;
    private LineRenderer lineRenderer;
    private Vector2 lineDirection;
    private Vector2 lineStartPoint;
    private Vector2 lineEndPoint;
    public float startOffset = 0.5f;
    public float lineLength = 5f;

    // Для отслеживания состояния
    private bool isRefracted = false;
    private Vector2 collisionPoint;
    private Vector2 newEndPoint;
    private EdgeCollider2D laserCollider;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        laserCollider = GetComponent<EdgeCollider2D>();

        // Если коллайдера нет - добавляем
        if (laserCollider == null)
        {
            laserCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        lineDirection = transform.right;
        UpdateLaser();
        UpdateCollider();
    }

    void Update()
    {
        // Обновляем только если не преломлено
        if (!isRefracted)
        {
            UpdateLaser();
            UpdateCollider();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isRefracted) return;

        Debug.Log("Столкновение с: " + other.name);

        // Получаем точку столкновения
        collisionPoint = other.ClosestPoint(transform.position);

        // Вычисляем новое направление (вниз)
        Vector2 newDirection = Vector2.down;

        // Вычисляем конечную точку в новом направлении
        newEndPoint = collisionPoint + newDirection * lineLength;

        // Обновляем LineRenderer с тремя точками
        lineRenderer.positionCount = 3;
        lineRenderer.SetPosition(0, lineStartPoint);
        lineRenderer.SetPosition(1, collisionPoint);
        lineRenderer.SetPosition(2, newEndPoint);

        // Обновляем коллайдер для новой части линии
        UpdateColliderAfterCollision();

        isRefracted = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (isRefracted)
        {
            Debug.Log("Выход из коллайдера: " + other.name);
            isRefracted = false;

            // Возвращаем линию к исходному состоянию
            UpdateLaser();
            UpdateCollider();
        }
    }

    void UpdateLaser()
    {
        Vector2 parentPosition = transform.parent != null ?
        (Vector2)transform.parent.position : (Vector2)transform.position;

        lineStartPoint = parentPosition + lineDirection * startOffset;
        lineEndPoint = lineStartPoint + lineDirection * lineLength;

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, lineStartPoint);
        lineRenderer.SetPosition(1, lineEndPoint);
    }

    void UpdateCollider()
    {
        // Исправленный синтаксис для EdgeCollider2D
        List<Vector2> colliderPoints = new List<Vector2>();
        colliderPoints.Add(lineStartPoint);
        colliderPoints.Add(lineEndPoint);

        laserCollider.SetPoints(colliderPoints);
    }

    void UpdateColliderAfterCollision()
    {
        // Исправленный синтаксис для EdgeCollider2D
        List<Vector2> colliderPoints = new List<Vector2>();
        colliderPoints.Add(collisionPoint);
        colliderPoints.Add(newEndPoint);

        laserCollider.SetPoints(colliderPoints);
    }
}