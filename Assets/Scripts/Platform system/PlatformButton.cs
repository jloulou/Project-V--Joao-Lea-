using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class PlatformButton : XRGrabInteractable
{
    public Platform platform;
    public int floor;
    private const float MOVEMENT_THRESHOLD = 0.01f;

    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        if (!base.IsSelectableBy(interactor) || platform == null)
            return false;

        // Check if platform is moving
        Vector3 currentPosition = platform.transform.position;
        Vector3 expectedPosition = platform.floors[platform.currentFloor].position;
        if (Vector3.Distance(currentPosition, expectedPosition) >= MOVEMENT_THRESHOLD)
            return false;

        // Check if interactor is on platform
        var interactorPosition = interactor.transform.position;
        return IsPositionOnPlatform(interactorPosition);
    }

    private bool IsPositionOnPlatform(Vector3 position)
    {
        Bounds platformBounds = platform.GetComponent<Collider>().bounds;
        return platformBounds.Contains(position);
    }

    protected override void OnSelectExiting(SelectExitEventArgs args)
    {
        base.OnSelectExiting(args);
        platform.nextFloor = floor;
    }
}