using UnityEngine;

public class FourthFloorState : PlatformState
{
    public override void OnEnter()
    {
        GetComponent<PlatformPlayerHandler>().SetCollidersState(true);

        targetY = platform.floors[4].position.y;
        platform.currentFloor = 4;
    }

    public override void CheckTransitionCondition()
    {
        if (platform.nextFloor == 0)
            controller.TransitionTo(GetComponent<GroundFloorState>());
        else if (platform.nextFloor == 1)
            controller.TransitionTo(GetComponent<FirstFloorState>());
        else if (platform.nextFloor == 2)
            controller.TransitionTo(GetComponent<SecondFloorState>());
        else if (platform.nextFloor == 3)
            controller.TransitionTo(GetComponent<ThirdFloorState>());
    }
}
