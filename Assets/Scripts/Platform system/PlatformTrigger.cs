using UnityEngine;
using UnityEngine.Events;

public class PlatformTrigger : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Reference to the Platform component")]
    public Platform platform;

    [Header("Platform Settings")]
    public int targetFloor;

    [Header("Events")]
    [Tooltip("Event called when platform starts moving")]
    public UnityEvent onPlatformTriggered;
    [Tooltip("Event called when platform reaches destination")]
    public UnityEvent onPlatformArrived;

    private bool isMoving = false;

    private void Awake()
    {
        // Validate the platform reference
        if (platform == null)
        {
            Debug.LogError("Platform reference is missing! Please assign it in the inspector.", this);
        }
    }

    // Public method that other objects can call to trigger the platform
    public void TriggerPlatform()
    {
        if (!isMoving && platform != null)
        {
            if (targetFloor >= 0 && targetFloor < platform.floors.Count)
            {
                isMoving = true;
                platform.SetNextFloor(targetFloor);
                onPlatformTriggered?.Invoke();
            }
            else
            {
                Debug.LogError($"Target floor {targetFloor} is out of range. Available floors: 0-{platform.floors.Count - 1}");
            }
        }
    }

    private void Update()
    {
        // Check if platform has reached its destination
        if (isMoving && platform != null && targetFloor < platform.floors.Count)
        {
            Vector3 targetPosition = platform.floors[targetFloor].position;
            float distanceToTarget = Vector3.Distance(platform.transform.position, targetPosition);

            if (distanceToTarget < 0.01f)  // Small threshold for floating point imprecision
            {
                isMoving = false;
                onPlatformArrived?.Invoke();
            }
        }
    }

    // Optional: Method to check if platform is currently moving
    public bool IsPlatformMoving()
    {
        return isMoving;
    }

    // Optional: Method to force stop the platform (if needed)
    public void StopPlatform()
    {
        if (platform != null)
        {
            isMoving = false;
            platform.SetNextFloor(platform.currentFloor);
        }
    }
}