using UnityEngine;
using System.Collections.Generic;
using System;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;

    public void SetNextFloor(int value)
    {
        nextFloor = value;
    }

    internal Vector3 GetFloorPosition(int currentFloor)
    {
        throw new NotImplementedException();
    }
}