using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlatformButton : XRSimpleInteractable
{
    public Platform platform;
    public int floor;

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        base.OnSelectEntered(args);
        platform.nextFloor = floor;
    }
}