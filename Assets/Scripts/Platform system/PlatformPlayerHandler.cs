using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class PlatformPlayerHandler : MonoBehaviour
{
    [Tooltip("Layer mask for detecting the player")]
    public LayerMask playerLayer;

    [Tooltip("Small offset above the platform to check for player")]
    public float detectionHeight = 0.1f;

    [Tooltip("Enable debug logs")]
    public bool debugMode = true;

    [Tooltip("Time in seconds player needs to stand on platform")]
    public float standingTimeThreshold = 3f;

    [Tooltip("Time in seconds after player leaves the platform")]
    public float exitTimeThreshold = 3f;

    [Tooltip("Event triggered when player stands on platform for the specified time")]
    public UnityEvent onPlayerStandingComplete;

    [Tooltip("Event triggered after player has been away for the specified time")]
    public UnityEvent onPlayerExitComplete;

    private List<BoxCollider> platformColliders = new List<BoxCollider>();
    private Transform currentPlayer;
    private Platform platform;
    private float currentStandingTime = 0f;
    private float currentExitTime = 0f;
    private bool isStandingTimerActive = false;
    private bool isExitTimerActive = false;
    private bool platformMoving = false;

    private void Start()
    {
        GetAllCollidersInChildren();
        platform = GetComponent<Platform>();

        // Initialize the UnityEvents if they haven't been already
        if (onPlayerStandingComplete == null)
            onPlayerStandingComplete = new UnityEvent();
        if (onPlayerExitComplete == null)
            onPlayerExitComplete = new UnityEvent();

        // Initially disable colliders when platform is stationary
        SetCollidersState(false);

        if (debugMode)
        {
            Debug.Log($"Platform Handler initialized with {platformColliders.Count} colliders");
            Debug.Log($"Player Layer Mask: {playerLayer.value}");
        }
    }

    public void OnPlatformStartMoving()
    {
        if (debugMode)
            Debug.Log("PlatformPlayerHandler: Platform started moving");

        platformMoving = true;
        SetCollidersState(true);  // Enable colliders when platform starts moving

        // If there's a player on the platform, ensure they're parented
        if (currentPlayer != null)
        {
            currentPlayer.transform.parent = transform;
            if (debugMode) Debug.Log("Parenting player to moving platform");
        }
    }

    public void OnPlatformStopMoving()
    {
        if (debugMode)
            Debug.Log("PlatformPlayerHandler: Platform stopped moving");

        platformMoving = false;
        SetCollidersState(false);  // Disable colliders when platform stops

        // If there's a player on the platform, maintain parenting
        if (currentPlayer != null)
        {
            // Keep the player parented even when stopped to maintain position
            if (debugMode) Debug.Log("Maintaining player parenting on stopped platform");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayer = other.transform;

            // Parent the player regardless of platform state
            currentPlayer.transform.parent = transform;
            if (debugMode) Debug.Log($"Player entered platform - Parenting (Platform Moving: {platformMoving})");

            // Start the standing timer
            currentStandingTime = 0f;
            isStandingTimerActive = true;

            // Stop the exit timer if it's running
            isExitTimerActive = false;
            currentExitTime = 0f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Always unparent when the player exits
            if (currentPlayer != null)
            {
                currentPlayer.transform.parent = null;
                if (debugMode) Debug.Log("Player exited platform - Unparenting");
            }

            // Reset standing timer
            currentStandingTime = 0f;
            isStandingTimerActive = false;

            // Start exit timer
            currentExitTime = 0f;
            isExitTimerActive = true;

            currentPlayer = null;
            if (debugMode) Debug.Log("Player exited platform");
        }
    }

    private void GetAllCollidersInChildren()
    {
        platformColliders.Clear();
        var colliders = GetComponentsInChildren<BoxCollider>();
        platformColliders.AddRange(colliders);

        if (debugMode)
            Debug.Log($"Found {platformColliders.Count} colliders in children");
    }

    private void SetCollidersState(bool enabled)
    {
        if (platformColliders.Count == 0)
        {
            Debug.LogWarning("No colliders found in platform!");
            return;
        }

        foreach (BoxCollider collider in platformColliders)
        {
            if (collider != null)
            {
                collider.enabled = enabled;
                if (debugMode) Debug.Log($"Collider {collider.name} enabled: {enabled}");
            }
        }
    }
}