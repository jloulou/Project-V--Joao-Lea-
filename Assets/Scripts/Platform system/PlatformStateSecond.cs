using UnityEngine;

public class SecondFloorState : PlatformState
{
    public override void Running()
    {
        Vector3 currentPos = transform.position;
        float distanceToTarget = targetY - currentPos.y;

        if (Mathf.Abs(distanceToTarget) < snapThreshold)
        {
            currentPos.y = targetY;
            transform.position = currentPos;
            GetComponent<PlatformPlayerHandler>().SetCollidersState(false);
            GetComponent<PlatformTrigger>().ResetTriggerState();  // Add this line
            Debug.Log("Stopped As Close Enough");
        }

        // Continue with existing movement code...
    }
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
