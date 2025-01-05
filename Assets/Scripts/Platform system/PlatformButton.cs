using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlatformButton : XRBaseInteractable
{
    public Platform platform;
    public int floor;
    private const float MOVEMENT_THRESHOLD = 0.1f; // Increased threshold for better detection
    private Collider platformCollider;
    private Vector3 localPosition;
    private Quaternion localRotation;

    [SerializeField]
    private bool debugMode = true; // Enable to see debug logs

    protected override void Awake()
    {
        base.Awake();

        if (platform == null)
        {
            Debug.LogError($"Platform reference not set on {gameObject.name}");
            return;
        }

        // Store local transform values relative to parent
        localPosition = transform.localPosition;
        localRotation = transform.localRotation;

        // Configure the rigidbody
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Start()
    {
        // Get platform collider if not already assigned
        if (platform != null && platformCollider == null)
        {
            platformCollider = platform.GetComponent<Collider>();
            if (platformCollider == null)
            {
                Debug.LogError($"No Collider found on platform for {gameObject.name}");
            }
        }
    }

    void Update()
    {
        // Maintain local position and rotation relative to parent
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);

        if (debugMode)
        {
            Debug.Log($"Button {gameObject.name} selected");
        }

        bool canActivate = CanActivateButton(args.interactorObject.transform);

        if (debugMode)
        {
            Debug.Log($"Can activate: {canActivate}");
        }

        if (!canActivate)
            return;

        if (platform != null)
        {
            platform.SetNextFloor(floor);
            if (debugMode)
            {
                Debug.Log($"Setting next floor to {floor}");
            }
        }
    }

    private bool CanActivateButton(Transform interactorTransform)
    {
        if (platform == null || platformCollider == null)
        {
            if (debugMode)
            {
                Debug.LogWarning($"Platform or collider missing on {gameObject.name}");
            }
            return false;
        }

        // Check if platform is at the correct position
        Vector3 currentPosition = platform.transform.position;
        Vector3 expectedPosition = platform.GetFloorPosition(platform.currentFloor);
        bool isPlatformAtCorrectPosition = Vector3.Distance(currentPosition, expectedPosition) < MOVEMENT_THRESHOLD;

        if (debugMode)
        {
            Debug.Log($"Platform position check: Current={currentPosition}, Expected={expectedPosition}, Distance={Vector3.Distance(currentPosition, expectedPosition)}");
        }

        // Check if player is on the platform
        bool isPlayerOnPlatform = platformCollider.bounds.Contains(interactorTransform.position);

        if (debugMode)
        {
            Debug.Log($"Player on platform: {isPlayerOnPlatform}");
        }

        return isPlatformAtCorrectPosition && isPlayerOnPlatform;
    }

    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        bool canPress = CanActivateButton(args.interactorObject.transform);
        UpdateVisualFeedback(canPress);

        if (debugMode)
        {
            Debug.Log($"Button {gameObject.name} hover entered. Can press: {canPress}");
        }
    }

    private void UpdateVisualFeedback(bool canPress)
    {
        // Optional: Add visual feedback here
        // For example, change the material color
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = canPress ? Color.green : Color.red;
        }
    }

    // Optional: Add visual hover exit feedback
    protected override void OnHoverExited(HoverExitEventArgs args)
    {
        base.OnHoverExited(args);
        if (TryGetComponent<Renderer>(out Renderer renderer))
        {
            renderer.material.color = Color.white;
        }
    }
}