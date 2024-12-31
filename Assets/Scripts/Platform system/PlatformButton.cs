using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlatformButton : XRGrabInteractable
{
    public Platform platform;
    public int floor;
    private const float MOVEMENT_THRESHOLD = 0.01f;
    private Collider platformCollider;

    void Start()
    {
        platformCollider = platform?.GetComponent<Collider>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (platform == null || platformCollider == null) return;

        Vector3 currentPosition = platform.transform.position;
        Vector3 expectedPosition = platform.floors[platform.currentFloor].position;

        if (Vector3.Distance(currentPosition, expectedPosition) >= MOVEMENT_THRESHOLD) return;
        if (!platformCollider.bounds.Contains(args.interactorObject.transform.position)) return;

        base.OnSelectEntered(args);
        platform.nextFloor = floor;
    }
}