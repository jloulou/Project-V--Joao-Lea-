using UnityEngine;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;
    public bool isMoving { get; private set; }

    private PlatformPlayerHandler playerHandler;

    private void Awake()
    {
        playerHandler = GetComponent<PlatformPlayerHandler>();
    }

    public void SetNextFloor(int value)
    {
        nextFloor = value;
        Debug.Log($"Next floor set to: {value}");
    }

    public void StartMoving()
    {
        if (!isMoving)  // Only set if not already moving
        {
            isMoving = true;
            Debug.Log("Platform.StartMoving called - Platform is now moving");
            if (playerHandler != null)
            {
                playerHandler.OnPlatformStartMoving();
            }
            else
            {
                Debug.LogError("PlayerHandler is null in Platform!");
            }
        }
    }

    public void StopMoving()
    {
        if (isMoving)  // Only set if currently moving
        {
            isMoving = false;
            Debug.Log("Platform.StopMoving called - Platform has stopped");
            if (playerHandler != null)
            {
                playerHandler.OnPlatformStopMoving();
            }
            else
            {
                Debug.LogError("PlayerHandler is null in Platform!");
            }
        }
    }
}