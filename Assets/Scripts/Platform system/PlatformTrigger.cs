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
    }
}