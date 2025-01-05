using UnityEngine;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;

    public void SetNextFloor(int value)
    {
        if (value >= 0 && value < floors.Count)
        {
            nextFloor = value;
        }
    }

    public Vector3 GetFloorPosition(int floorIndex)
    {
        if (floorIndex >= 0 && floorIndex < floors.Count)
        {
            return floors[floorIndex].position;
        }

        // If floor index is invalid, return current position
        return transform.position;
    }
}