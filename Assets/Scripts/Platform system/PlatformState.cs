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

    protected void Awake()
    {
        platform = GetComponent<Platform>();
    }

    public override void Running()
    {
        Vector3 currentPos = transform.position;
        float distanceToTarget = targetY - currentPos.y;

        // Snap if very close
        if (Mathf.Abs(distanceToTarget) < snapThreshold)
        {
            currentPos.y = targetY;
            transform.position = currentPos;
            return;
        }

        // Smooth movement with gradual slowdown
        float speed = moveSpeed * (Mathf.Abs(distanceToTarget) / smoothing);
        speed = Mathf.Clamp(speed, 0.1f, moveSpeed);

        float direction = distanceToTarget > 0 ? 1f : -1f;
        currentPos.y += speed * direction * Time.fixedDeltaTime;
        transform.position = currentPos;
    }
}