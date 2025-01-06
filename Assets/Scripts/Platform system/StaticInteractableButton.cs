using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.Events;

public class StaticInteractableButton : XRBaseInteractable
{
    [SerializeField]
    private UnityEvent onButtonSelected;

    [Tooltip("Reference to the platform this button controls")]
    [SerializeField]
    private Platform platform;

    [Tooltip("Maximum distance to consider platform at a floor")]
    [SerializeField]
    private float floorPositionThreshold = 0.1f;

    private Vector3 localPosition;
    private Quaternion localRotation;
    private Rigidbody rb;
    private PlatformPlayerHandler platformPlayerHandler;

    protected override void Awake()
    {
        base.Awake();

        // Store local transform values
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;

        // Configure the rigidbody
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Get reference to platform player handler
        if (platform != null)
        {
            platformPlayerHandler = platform.GetComponent<PlatformPlayerHandler>();
        }
    }

    private void Update()
    {
        // Maintain local position and rotation relative to parent
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (CanInteractWithButton())
        {
            base.OnSelectEntered(args);
            onButtonSelected?.Invoke();
            ShowUIKeyboard();
        }
    }

    protected override void OnSelectExited(SelectExitEventArgs args)
    {
        base.OnSelectExited(args);
    }

    private bool CanInteractWithButton()
    {
        if (platform == null || platformPlayerHandler == null)
        {
            Debug.LogWarning("Platform or PlatformPlayerHandler reference is missing!");
            return false;
        }

        // Use the new public method instead of accessing currentPlayer directly
        bool isPlayerOnPlatform = platformPlayerHandler.IsPlayerOnPlatform();

        // Check if platform is at a valid floor position
        bool isPlatformAtFloor = IsAtAnyFloor();

        return isPlayerOnPlatform && isPlatformAtFloor;
    }

    private bool IsAtAnyFloor()
    {
        Vector3 currentPosition = platform.transform.position;

        // Check against all floor positions
        for (int i = 0; i < platform.floors.Count; i++)
        {
            Vector3 floorPosition = platform.GetFloorPosition(i);
            if (Vector3.Distance(currentPosition, floorPosition) <= floorPositionThreshold)
            {
                return true;
            }
        }

        return false;
    }

    private void ShowUIKeyboard()
    {
        // Add your UI keyboard activation logic here
    }
}