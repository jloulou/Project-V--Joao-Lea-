using UnityEngine;
using StateMachine;

public abstract class PlatformState : State
{
    protected Platform platform;
    protected float targetY;

    [SerializeField]
    protected float moveSpeed = 2f;

    [SerializeField]
    protected float snapThreshold = 0.01f;

    [SerializeField]
    [Tooltip("Higher values = smoother slowdown")]
    protected float smoothing = 1f;

    protected bool hasStartedMoving = false;
    protected bool hasReachedTarget = false;

    protected void Awake()
    {
        platform = GetComponent<Platform>();
    }

    public override void OnEnter()
    {
        if (platform == null)
        {
            Debug.LogError("Platform reference is null in PlatformState!");
            return;
        }

        // Reset state flags
        hasStartedMoving = false;
        hasReachedTarget = false;

        Debug.Log("PlatformState.OnEnter called");
    }

    public override void OnExit()
    {
        if (platform == null)
        {
            Debug.LogError("Platform reference is null in PlatformState!");
            return;
        }

        // Only stop moving if we haven't already
        if (platform.isMoving)
        {
            platform.StopMoving();
        }

        Debug.Log("PlatformState.OnExit called");
    }

    public override void Running()
    {
        if (platform == null) return;

        // Start moving on first Running() call
        if (!hasStartedMoving)
        {
            platform.StartMoving();
            hasStartedMoving = true;
            return;
        }

        Vector3 currentPos = transform.position;
        float distanceToTarget = targetY - currentPos.y;

        // Check if we've reached the target
        if (Mathf.Abs(distanceToTarget) < snapThreshold)
        {
            if (!hasReachedTarget)
            {
                currentPos.y = targetY;
                transform.position = currentPos;
                platform.StopMoving();
                hasReachedTarget = true;
                Debug.Log("Platform reached target position");
            }
            return;
        }

        // Smooth movement with gradual slowdown
        float speed = moveSpeed * (Mathf.Abs(distanceToTarget) / smoothing);
        speed = Mathf.Clamp(speed, 0.1f, moveSpeed);

        float direction = distanceToTarget > 0 ? 1f : -1f;
        currentPos.y += speed * direction * Time.fixedDeltaTime;
        transform.position = currentPos;
    }

    public override void CheckTransitionCondition()
    {
        // Implementation will be in derived states
    }
}