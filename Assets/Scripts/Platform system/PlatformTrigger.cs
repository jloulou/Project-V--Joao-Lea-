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

    public void TriggerPlatform()
    {
        Debug.Log($"TriggerPlatform called. isMoving: {isMoving}");

        if (!isMoving)
        {
            isMoving = true;
            platform.nextFloor = targetFloor;
            onPlatformTriggered?.Invoke();
            Debug.Log("Platform movement triggered");
        }
        else
        {
            Debug.Log("Platform trigger ignored because isMoving is true");
        }
    }

    // Add this method to manually reset the state
    public void ResetTriggerState()
    {
        Debug.Log($"Resetting trigger state. Old isMoving: {isMoving}");
        isMoving = false;
    }

    public bool IsPlatformMoving()
    {
        return isMoving;
    }

    public void StopPlatform()
    {
        isMoving = false;
        platform.nextFloor = platform.currentFloor;
    }
}