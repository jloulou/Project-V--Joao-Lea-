using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundFloorState : PlatformState
{
    public override void OnEnter()
    {
        targetY = platform.floors[0].position.y;
        platform.currentFloor = 0;
    }

    public override void CheckTransitionCondition()
    {
        if (platform.nextFloor == 1)
            controller.TransitionTo(GetComponent<FirstFloorState>());
        else if (platform.nextFloor == 2)
            controller.TransitionTo(GetComponent<SecondFloorState>());
        else if (platform.nextFloor == 3)
            controller.TransitionTo(GetComponent<ThirdFloorState>());
        else if (platform.nextFloor == 4)
            controller.TransitionTo(GetComponent<FourthFloorState>());
    }
}