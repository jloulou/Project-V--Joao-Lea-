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

    private List<BoxCollider> movementColliders = new List<BoxCollider>();
    private List<BoxCollider> mainPlatformColliders = new List<BoxCollider>();
    private Transform currentPlayer;
    private Platform platform;
    private float currentStandingTime = 0f;
    private float currentExitTime = 0f;
    private bool isStandingTimerActive = false;
    private bool isExitTimerActive = false;
    private bool platformMoving = false;

    private void Start()
    {
        GetColliders();
        platform = GetComponent<Platform>();

        // Initialize the UnityEvents if they haven't been already
        if (onPlayerStandingComplete == null)
            onPlayerStandingComplete = new UnityEvent();
        if (onPlayerExitComplete == null)
            onPlayerExitComplete = new UnityEvent();

        // Initially disable only the movement colliders
        SetMovementCollidersState(false);

        if (debugMode)
        {
            Debug.Log($"Platform Handler initialized with {mainPlatformColliders.Count} main colliders and {movementColliders.Count} movement colliders");
            Debug.Log($"Player Layer Mask: {playerLayer.value}");
        }
    }

    private void GetColliders()
    {
        movementColliders.Clear();
        mainPlatformColliders.Clear();

        // Get all colliders attached directly to this GameObject
        var platformColliders = GetComponents<BoxCollider>();
        mainPlatformColliders.AddRange(platformColliders);

        // Get all child colliders
        var childColliders = GetComponentsInChildren<BoxCollider>();
        foreach (var collider in childColliders)
        {
            // Only add colliders that are on child objects
            if (collider.gameObject != gameObject)
            {
                movementColliders.Add(collider);
            }
        }

        if (debugMode)
        {
            Debug.Log($"Found {mainPlatformColliders.Count} main platform colliders");
            Debug.Log($"Found {movementColliders.Count} movement colliders in children");
        }
    }

    private void SetMovementCollidersState(bool enabled)
    {
        if (movementColliders.Count == 0)
        {
            Debug.LogWarning("No movement colliders found in platform children!");
            return;
        }

        foreach (BoxCollider collider in movementColliders)
        {
            if (collider != null)
            {
                collider.enabled = enabled;
                if (debugMode) Debug.Log($"Movement collider {collider.name} enabled: {enabled}");
            }
        }
    }

    public void OnPlatformStartMoving()
    {
        if (debugMode)
            Debug.Log("PlatformPlayerHandler: Platform started moving");

        platformMoving = true;
        SetMovementCollidersState(true);  // Enable movement colliders

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
        SetMovementCollidersState(false);  // Disable movement colliders

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

    private void Update()
    {
        // Standing timer logic
        if (isStandingTimerActive && currentPlayer != null)
        {
            currentStandingTime += Time.deltaTime;

            if (currentStandingTime >= standingTimeThreshold)
            {
                if (debugMode)
                    Debug.Log("Player standing time threshold reached!");

                onPlayerStandingComplete.Invoke();
                isStandingTimerActive = false;
            }
        }

        // Exit timer logic
        if (isExitTimerActive)
        {
            currentExitTime += Time.deltaTime;

            if (currentExitTime >= exitTimeThreshold)
            {
                if (debugMode)
                    Debug.Log("Player exit time threshold reached!");

                onPlayerExitComplete.Invoke();
                isExitTimerActive = false;
            }
        }
    }
}