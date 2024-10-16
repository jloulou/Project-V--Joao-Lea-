using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FloatingThing : MonoBehaviour
{
    [Header("Movement Animation")]
    public float movementSpeed = 0.001f;
    public float movementAmount = 0.01f;
    
    [Header("Rotation Animation")]
    public float rotationXSpeed = 0;
    public float rotationYSpeed = 0;
    public float rotationZSpeed = 0;
    
    [Header("Grab")]
    public bool isGrabbed = false;
    
    private double radians;
    private Vector3 initialPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        radians = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isGrabbed)
            return;
        
        if (radians > (Math.PI * 2))
            radians = 0;
        double movement = Math.Sin(radians) * movementAmount;
        
        // Movement
        Vector3 pos = transform.position;
        pos.y = initialPosition.y + (float)movement;
        transform.position = pos;
        
        // Rotation
        transform.Rotate(rotationXSpeed, rotationYSpeed, rotationZSpeed);

        radians += movementSpeed;
    }
}
