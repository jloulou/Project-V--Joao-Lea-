using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformPlayerHandler : MonoBehaviour
{
    [Tooltip("Layer mask for detecting the player")]
    public LayerMask playerLayer;

    [Tooltip("Small offset above the platform to check for player")]
    public float detectionHeight = 0.1f;

    [Tooltip("Enable debug logs")]
    public bool debugMode = true;

    private BoxCollider[] platformColliders;
    private Transform currentPlayer;
    private Platform platform;

    private void Start()
    {
        platformColliders = GetComponentsInChildren<BoxCollider>();
        platform = GetComponent<Platform>();
        SetCollidersState(true);

        if (debugMode)
        {
            Debug.Log($"Platform Handler initialized with {platformColliders.Length} colliders");
            Debug.Log($"Player Layer Mask: {playerLayer.value}");
        }
    }

    private void SetCollidersState(bool enabled)
    {
        foreach (BoxCollider collider in platformColliders)
        {
            collider.enabled = enabled;
            if (debugMode) Debug.Log($"Collider {collider.name} enabled: {enabled}");
        }
    }

    private void FixedUpdate()
    {
        if (platformColliders.Length == 0) return;

        BoxCollider mainCollider = platformColliders[0];
        Vector3 boxCenter = transform.position + mainCollider.center;
        boxCenter.y += mainCollider.size.y / 2 + detectionHeight / 2;

        Vector3 boxHalfExtents = mainCollider.size;
        boxHalfExtents.y = detectionHeight / 2;

        // Check for player above platform
        Collider[] hitColliders = Physics.OverlapBox(
            boxCenter,
            boxHalfExtents,
            transform.rotation,
            playerLayer
        );

        if (debugMode && hitColliders.Length > 0)
        {
            Debug.Log($"Detected object: {hitColliders[0].name} on layer {hitColliders[0].gameObject.layer}");
        }

        // If we found the player and it's not already parented
        if (hitColliders.Length > 0 && hitColliders[0].transform != currentPlayer)
        {
            currentPlayer = hitColliders[0].transform;
            currentPlayer.parent = transform;
            if (debugMode) Debug.Log($"Parented player {currentPlayer.name} to platform");
        }
        // If we lost the player
        else if (hitColliders.Length == 0 && currentPlayer != null)
        {
            if (debugMode) Debug.Log($"Unparenting player {currentPlayer.name} from platform");
            currentPlayer.parent = null;
            currentPlayer = null;
        }
    }

    private void OnDrawGizmos()
    {
        if (!debugMode || platformColliders == null || platformColliders.Length == 0) return;

        BoxCollider mainCollider = platformColliders[0];
        Vector3 boxCenter = transform.position + mainCollider.center;
        boxCenter.y += mainCollider.size.y / 2 + detectionHeight / 2;

        Vector3 boxHalfExtents = mainCollider.size;
        boxHalfExtents.y = detectionHeight / 2;

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(boxCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}