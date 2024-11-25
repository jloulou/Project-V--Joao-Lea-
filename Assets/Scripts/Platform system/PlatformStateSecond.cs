using UnityEngine;

public class SecondFloorState : PlatformState
{
    public override void OnEnter()
    {
        GetComponent<PlatformPlayerHandler>().SetCollidersState(true);

        targetY = platform.floors[2].position.y;
        platform.currentFloor = 2;
    }

    public override void CheckTransitionCondition()
    {
        if (platform.nextFloor == 0)
            controller.TransitionTo(GetComponent<GroundFloorState>());
        else if (platform.nextFloor == 1)
            controller.TransitionTo(GetComponent<FirstFloorState>());
        else if (platform.nextFloor == 3)
            controller.TransitionTo(GetComponent<ThirdFloorState>());
        else if (platform.nextFloor == 4)
            controller.TransitionTo(GetComponent<FourthFloorState>());
    }
}
