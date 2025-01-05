using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public Transform player;

    [Header("Distance Thresholds")]
    public float runDistance = 5f;    // Start running when player is closer than this
    public float walkDistance = 10f;  // Start walking when player is closer than this

    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float terrainCheckDistance = 0.5f; // Distance to check below for terrain
    public LayerMask terrainLayer; // Layer mask for terrain detection

    // Animation parameter names
    private readonly string isWalking = "IsWalking";
    private readonly string isRunning = "IsRunning";

    private void Start()
    {
        // Ensure we have references
        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        // Calculate distance to player
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Handle animation and movement based on distance
        if (distanceToPlayer <= runDistance)
        {
            // Too close - Run away
            animator.SetBool(isWalking, false);
            animator.SetBool(isRunning, true);
            MoveAwayFromPlayer(runSpeed);
        }
        else if (distanceToPlayer <= walkDistance)
        {
            // Getting close - Walk away
            animator.SetBool(isWalking, true);
            animator.SetBool(isRunning, false);
            MoveAwayFromPlayer(walkSpeed);
        }
        else
        {
            // Safe distance - Stand idle
            animator.SetBool(isWalking, false);
            animator.SetBool(isRunning, false);
        }
    }

    private void MoveAwayFromPlayer(float speed)
    {
        // Calculate direction away from player
        Vector3 directionFromPlayer = (transform.position - player.position).normalized;

        // Calculate potential new position
        Vector3 targetPosition = transform.position + directionFromPlayer * speed * Time.deltaTime;

        // Raycast to find terrain height at new position
        RaycastHit hit;
        Ray ray = new Ray(targetPosition + Vector3.up * 10f, Vector3.down);

        if (Physics.Raycast(ray, out hit, 20f, terrainLayer))
        {
            // Set the y position to the terrain height
            targetPosition.y = hit.point.y;

            // Check if the slope is too steep
            if (Vector3.Angle(hit.normal, Vector3.up) < 45f)
            {
                // Move to the new position
                transform.position = targetPosition;

                // Rotate to face away from player
                transform.rotation = Quaternion.LookRotation(directionFromPlayer);
            }
            else
            {
                // If slope is too steep, try to find an alternative direction
                TryAlternativeDirection(speed);
            }
        }
    }

    private void TryAlternativeDirection(float speed)
    {
        // Try moving at 45-degree angles from the direct escape route
        float[] angles = { 45f, -45f, 90f, -90f };

        foreach (float angle in angles)
        {
            Vector3 directionFromPlayer = (transform.position - player.position).normalized;
            Vector3 altDirection = Quaternion.Euler(0, angle, 0) * directionFromPlayer;
            Vector3 targetPosition = transform.position + altDirection * speed * Time.deltaTime;

            RaycastHit hit;
            Ray ray = new Ray(targetPosition + Vector3.up * 10f, Vector3.down);

            if (Physics.Raycast(ray, out hit, 20f, terrainLayer))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) < 45f)
                {
                    targetPosition.y = hit.point.y;
                    transform.position = targetPosition;
                    transform.rotation = Quaternion.LookRotation(altDirection);
                    return;
                }
            }
        }
    }
}