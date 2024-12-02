using UnityEngine;
using System.Collections.Generic;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public int currentFloor;
    public int nextFloor;

    public void SetNextFloor(int value)
    {
        nextFloor = value;
    }
}