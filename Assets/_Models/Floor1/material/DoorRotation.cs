using UnityEngine;

public class DoorRotation : MonoBehaviour
{
    public Transform player; // Reference to the player
    public float detectionRange = 0.5f; // The range to detect the player
    public float rotationSpeed = 2f; // Speed of the rotation
    private bool isRotating = false; // Flag to start rotating the door
    private Quaternion targetRotation; // The target rotation of the door

    private void Start()
    {
        // Set the target rotation to 150 degrees on the Y-axis (initially at the current rotation)
        targetRotation = Quaternion.Euler(0, 150, 0);
    }

    private void Update()
    {
        // Calculate the distance between the player and the door
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        // Check if the player is within the detection range and the door is not already rotating
        if (distanceToPlayer <= detectionRange && !isRotating)
        {
            // Start rotating the door
            isRotating = true;
        }

        // Rotate the door if it's supposed to
        if (isRotating)
        {
            // Smoothly rotate the door towards the target rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Check if the door has reached the target rotation
            if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f)
            {
                // Stop rotating when the target rotation is reached
                isRotating = false;
            }
        }
    }
}

