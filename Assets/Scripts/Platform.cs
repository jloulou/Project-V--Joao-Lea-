using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public List<Transform> floors;
    public GameObject Everything;

    public int currentFloor;
    public int nextFloor;
    public float movementSpeed;

    void Start()
    {


    }

    private void FixedUpdate()
    {
        if (currentFloor != nextFloor)
        {
            Vector3 pos = Everything.transform.position;
            pos.y -= movementSpeed;
            Everything.transform.position = pos;

            if ((transform.position.y >= (floors[nextFloor].position.y - 0.1)) &&
                (transform.position.y <= (floors[nextFloor].position.y + 0.1)))
            {
                currentFloor = nextFloor;
            }
        }
    }
}