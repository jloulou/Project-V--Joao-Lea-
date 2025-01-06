using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class PlatformButton : XRGrabInteractable
{
    public Platform platform;
    public int floor;
    private PlatformPlayerHandler playerHandler;

    protected override void Awake()
    {
        base.Awake();
        playerHandler = platform.GetComponent<PlatformPlayerHandler>();
    }

    protected override void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (platform.IsAtValidFloor() && playerHandler.HasPlayerOnPlatform())
        {
            base.OnSelectEntered(args);
            platform.nextFloor = floor;
        }
    }
}