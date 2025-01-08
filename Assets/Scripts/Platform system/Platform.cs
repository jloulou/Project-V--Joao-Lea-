// Platform.cs
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;
    public UnityEvent onPlatformArrived;  // Add this line

    private void Awake()
    {
        // Initialize the event if it hasn't been
        if (onPlatformArrived == null)
            onPlatformArrived = new UnityEvent();
    }

    public void SetNextFloor(int value)
    {
        nextFloor = value;
    }

    internal Vector3 GetFloorPosition(int currentFloor)
    {
        if (currentFloor >= 0 && currentFloor < floors.Count)
            return floors[currentFloor].position;
        return Vector3.zero;
    }
}

