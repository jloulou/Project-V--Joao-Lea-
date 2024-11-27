using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInputController : MonoBehaviour
{
    private Platform platform;

    void Start()
    {
        platform = GetComponent<Platform>();
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha0))
            platform.nextFloor = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            platform.nextFloor = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            platform.nextFloor = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            platform.nextFloor = 3;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            platform.nextFloor = 4;
    }
}