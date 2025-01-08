using UnityEngine;

public class FirstFloorState : PlatformState
{
    public override void Running()
    {
        base.Running();

        Vector3 currentPos = transform.position;
        float distanceToTarget = targetY - currentPos.y;

        // When platform reaches destination
        if (Mathf.Abs(distanceToTarget) < snapThreshold)
        {
            currentPos.y = targetY;
            transform.position = currentPos;
            GetComponent<PlatformPlayerHandler>().SetCollidersState(false);
            platform.onPlatformArrived?.Invoke();  // Add this line
            Debug.Log("Stopped As Close Enough");
        }
    }
  


    public override void OnEnter()
    {
        GetComponent<PlatformPlayerHandler>().SetCollidersState(true);

        targetY = platform.floors[1].position.y;
        platform.currentFloor = 1;
    }

    public override void CheckTransitionCondition()
    {
        if (platform.nextFloor == 0)
            controller.TransitionTo(GetComponent<GroundFloorState>());
        else if (platform.nextFloor == 2)
            controller.TransitionTo(GetComponent<SecondFloorState>());
        else if (platform.nextFloor == 3)
            controller.TransitionTo(GetComponent<ThirdFloorState>());
        else if (platform.nextFloor == 4)
            controller.TransitionTo(GetComponent<FourthFloorState>());
    }
}