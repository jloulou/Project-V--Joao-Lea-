using UnityEngine;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;
    private bool isAtValidFloor = false;

    private void Update()
    {
        CheckPosition();
    }

    private void CheckPosition()
    {
        isAtValidFloor = false;
        foreach (Transform floor in floors)
        {
            if (Vector3.Distance(transform.position, floor.position) < 0.01f)
            {
                isAtValidFloor = true;
                break;
            }
        }
    }

    public bool IsAtValidFloor()
    {
        return isAtValidFloor;
    }

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
        return transform.position;
    }
}