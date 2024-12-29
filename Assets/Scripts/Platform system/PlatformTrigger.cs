using UnityEngine;
using UnityEngine.Events;

public class PlatformTrigger : MonoBehaviour
{
    [Header("Platform Settings")]
    public Platform platform;
    public int targetFloor;

    [Header("Trigger Settings")]
    public string triggerTag = "Player";  // Objects with this tag can trigger the platform
    public bool requiresStay = false;     // If true, object must stay in trigger to activate

    [Header("Events")]
    public UnityEvent onTriggerActivated;
    public UnityEvent onTriggerDeactivated;

    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggerTag) && !isTriggered)
        {
            ActivatePlatform();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(triggerTag) && isTriggered && requiresStay)
        {
            DeactivatePlatform();
        }
    }

    public void ActivatePlatform()
    {
        isTriggered = true;
        platform.nextFloor = targetFloor;
        onTriggerActivated?.Invoke();
    }

    public void DeactivatePlatform()
    {
        isTriggered = false;
        onTriggerDeactivated?.Invoke();
    }

    // Public method to trigger the platform programmatically
    public void TriggerPlatform()
    {
        ActivatePlatform();
    }

    // Public method to reset the trigger
    public void ResetTrigger()
    {
        DeactivatePlatform();
    }
}
