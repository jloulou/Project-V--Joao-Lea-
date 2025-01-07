// PlatformTrigger.cs
using UnityEngine;
using UnityEngine.Events;

public class PlatformTrigger : MonoBehaviour
{
    [Header("Platform Settings")]
    public Platform platform;
    public int targetFloor;

    [Header("Events")]
    [Tooltip("Event called when platform starts moving")]
    public UnityEvent onPlatformTriggered;
    [Tooltip("Event called when platform reaches destination")]
    public UnityEvent onPlatformArrived;

    private bool isMoving = false;

    private void OnEnable()
    {
        // Subscribe to platform's arrival event if it exists
        if (platform != null)
        {
            onPlatformArrived.AddListener(ResetTrigger);
        }
    }

    private void OnDisable()
    {
        // Clean up event subscription
        if (platform != null)
        {
            onPlatformArrived.RemoveListener(ResetTrigger);
        }
    }

    // Public method that other objects can call to trigger the platform
    public void TriggerPlatform()
    {
        if (!isMoving)
        {
            isMoving = true;
            platform.nextFloor = targetFloor;
            onPlatformTriggered?.Invoke();
        }
    }

    // Reset the trigger when the platform arrives
    private void ResetTrigger()
    {
        isMoving = false;
    }

    // Optional: Method to check if platform is currently moving
    public bool IsPlatformMoving()
    {
        return isMoving;
    }

    // Optional: Method to force stop the platform (if needed)
    public void StopPlatform()
    {
        isMoving = false;
        platform.nextFloor = platform.currentFloor;
        onPlatformArrived?.Invoke();
    }
}