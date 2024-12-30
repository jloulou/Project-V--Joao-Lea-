using UnityEngine;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;
    public float floorHeight = 3f;

    public void SetNextFloor(int value)
    {
        nextFloor = value;
    }

    internal Vector3 GetFloorPosition(int floor)
    {
        return new Vector3(transform.position.x, floor * floorHeight, transform.position.z);
    }
}