using UnityEngine;
using System.Collections.Generic;

public class FloatingStarPath : MonoBehaviour
{
    [Header("Star Movement")]
    public float moveSpeed = 3f;
    public float floatHeight = 1.5f;

    [Header("Player Check")]
    public Transform player;
    public float maxDistanceFromPlayer = 10f;

    [Header("Floating Animation")]
    public float bobAmplitude = 0.2f;
    public float bobSpeed = 2f;

    [Header("Smooth Turning")]
    public float turnSpeed = 5f; // HIGHER = faster snapping, LOWER = smoother

    private List<Transform> pathPoints;
    private int currentIndex = 0;
    private bool isMoving = false;

    private Vector3 basePosition;
    private float bobOffsetTime;

    void Start()
    {
        basePosition = transform.position;
        bobOffsetTime = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (isMoving && pathPoints != null && pathPoints.Count > 0)
        {
            float distance = Vector3.Distance(basePosition, player.position);
            if (distance <= maxDistanceFromPlayer)
                MoveAlongPath();
        }

        // Floating Bob
        float bob = Mathf.Sin((Time.time + bobOffsetTime) * bobSpeed) * bobAmplitude;
        Vector3 bobbedPosition = basePosition + Vector3.up * bob;
        transform.position = bobbedPosition;
    }

    private void MoveAlongPath()
    {
        if (currentIndex >= pathPoints.Count)
        {
            isMoving = false;
            return;
        }

        Transform targetPoint = pathPoints[currentIndex];
        Vector3 targetPos = targetPoint.position + Vector3.up * floatHeight;

        // Move base position
        basePosition = Vector3.MoveTowards(
            basePosition,
            targetPos,
            moveSpeed * Time.deltaTime
        );

        // SMOOTH TURNING ⭐
        Vector3 direction = (targetPos - transform.position).normalized;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * turnSpeed
            );
        }

        // Check arrival
        if (Vector3.Distance(basePosition, targetPos) < 0.1f)
        {
            currentIndex++;

            if (currentIndex >= pathPoints.Count)
            {
                isMoving = false;
                TutorialManager.Instance?.OnStarReachedTarget();
            }
        }
    }

    public void FollowPath(List<Transform> newPath)
    {
        pathPoints = newPath;
        currentIndex = 0;
        isMoving = (pathPoints != null && pathPoints.Count > 0);
        basePosition = transform.position;
    }

    public void SnapToPlayer()
    {
        if (player == null) return;
        basePosition = player.position + Vector3.up * floatHeight;
        transform.position = basePosition;
    }
}
