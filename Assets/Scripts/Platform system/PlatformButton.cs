using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlatformButton : XRBaseInteractable
{
    public Platform platform;
    public int floor;
    private const float MOVEMENT_THRESHOLD = 0.01f;
    private Collider platformCollider;
    private bool isMoving = false;

    protected override void Awake()
    {
        base.Awake();
        // Force the interactable to be non-kinematic and non-movable
        transform.SetParent(null, true);
        if (TryGetComponent<Rigidbody>(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

    void Start()
    {
        platformCollider = platform?.GetComponent<Collider>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (!CanActivateButton(args.interactorObject.transform))
            return;

        base.OnSelectEntered(args);
        platform.SetNextFloor(floor);
    }

    private bool CanActivateButton(Transform interactorTransform)
    {
        if (platform == null || platformCollider == null)
            return false;

        // Check if platform is at the correct position
        Vector3 currentPosition = platform.transform.position;
        Vector3 expectedPosition = platform.GetFloorPosition(platform.currentFloor);
        bool isPlatformAtCorrectPosition = Vector3.Distance(currentPosition, expectedPosition) < MOVEMENT_THRESHOLD;

        // Check if player is on the platform
        bool isPlayerOnPlatform = platformCollider.bounds.Contains(interactorTransform.position);

        // Only allow activation if all conditions are met
        return isPlatformAtCorrectPosition && isPlayerOnPlatform;
    }

    // Optional: Visual feedback when button can/cannot be pressed
    protected override void OnHoverEntered(HoverEnterEventArgs args)
    {
        base.OnHoverEntered(args);
        UpdateVisualFeedback(CanActivateButton(args.interactorObject.transform));
    }

    private void UpdateVisualFeedback(bool canPress)
    {
        // Add visual feedback here (e.g., change material color)
        // You can add a material component reference and modify its color
        // or use any other visual indicator
    }
}