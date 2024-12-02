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

    private List<BoxCollider> platformColliders = new List<BoxCollider>();
    private Transform currentPlayer;
    private Platform platform;

    private void Start()
    {
        GetAllCollidersInChildren();
        platform = GetComponent<Platform>();

        if (debugMode)
        {
            Debug.Log($"Platform Handler initialized with {platformColliders.Count} colliders");
            Debug.Log($"Player Layer Mask: {playerLayer.value}");
        }
    }

    private void GetAllCollidersInChildren()
    {
        for (int i = 0; i < 4; ++i)
        {
            platformColliders.Add(transform.GetChild(i).GetComponent<BoxCollider>());
        }

    }

    public void SetCollidersState(bool enabled)
    {
        foreach (BoxCollider collider in platformColliders)
        {
            collider.enabled = enabled;
            if (debugMode) Debug.Log($"Collider {collider.name} enabled: {enabled}");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayer = other.transform;

            currentPlayer.transform.parent = transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            currentPlayer.transform.parent = null;

            currentPlayer = null;
        }
    }

}