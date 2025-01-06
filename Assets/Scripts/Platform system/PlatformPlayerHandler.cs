using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    public bool IsPlayerOnPlatform()
    {
        return currentPlayer != null;
    }

    private void Start()
    {
        // Get all box colliders that are children of this object
        platformColliders.AddRange(GetComponentsInChildren<BoxCollider>());

        platform = GetComponent<Platform>();

        if (onPlayerStandingComplete == null)
            onPlayerStandingComplete = new UnityEvent();
        if (onPlayerExitComplete == null)
            onPlayerExitComplete = new UnityEvent();

        if (debugMode)
        {
            Debug.Log($"Platform Handler initialized with {platformColliders.Count} colliders");
            Debug.Log($"Player Layer Mask: {playerLayer.value}");
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

    public void SetCollidersState(bool enabled)
    {
        foreach (BoxCollider collider in platformColliders)
        {
            collider.enabled = enabled;
            if (debugMode) Debug.Log($"Collider {collider.name} enabled: {enabled}");
        }

        if (currentPlayer)
        {
            currentPlayer.transform.parent = enabled ? transform : null;
            Debug.Log(enabled);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayer = other.transform;
            currentPlayer.transform.parent = transform;

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
            currentPlayer.transform.parent = null;

            // Reset standing timer
            currentStandingTime = 0f;
            isStandingTimerActive = false;

            // Start exit timer
            currentExitTime = 0f;
            isExitTimerActive = true;

            //currentPlayer = null;
        }
    }
}