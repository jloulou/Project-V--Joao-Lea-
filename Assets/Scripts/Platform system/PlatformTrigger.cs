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

    void Awake()
    {
        Debug.Log($"PlatformTrigger Awake on {gameObject.name}. Initial isMoving: {isMoving}");
    }

    void Start()
    {
        Debug.Log($"PlatformTrigger Start on {gameObject.name}. isMoving: {isMoving}, targetFloor: {targetFloor}");
    }

    public void TriggerPlatform()
    {
        Debug.Log($"TriggerPlatform called on {gameObject.name}. isMoving: {isMoving}, targetFloor: {targetFloor}");

        if (!isMoving)
        {
            Debug.Log($"Setting platform.nextFloor to {targetFloor}");
            isMoving = true;
            platform.nextFloor = targetFloor;
            onPlatformTriggered?.Invoke();
            Debug.Log($"Platform movement triggered on {gameObject.name}");
        }
        else
        {
            Debug.Log($"Platform trigger ignored on {gameObject.name} because isMoving is true");
        }
    }

    public void ResetTriggerState()
    {
        Debug.Log($"Resetting trigger state on {gameObject.name}. Old isMoving: {isMoving}");
        isMoving = false;
        Debug.Log($"New isMoving state: {isMoving}");
    }

    public bool IsPlatformMoving()
    {
        return isMoving;
    }

    public void StopPlatform()
    {
        Debug.Log($"StopPlatform called on {gameObject.name}");
        isMoving = false;
        platform.nextFloor = platform.currentFloor;
    }
}