using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider wheelCollider; // Reference to the WheelCollider
    
    private Vector3 _lastPosition; // The last position of the wheel

    private void Start() => 
        InitializeWheel();


    private void Update() => 
        RotateWheel();

    
    /// <summary>
    /// Rotates the wheel based on how fast it has moved
    /// </summary>
    private void RotateWheel()
    {
        var movement = transform.position - _lastPosition;

        var distance = Vector3.Dot(movement, transform.parent.forward);
        var rotationAngle = (distance / (2 * Mathf.PI * wheelCollider.radius)) * 360f;
        
        transform.Rotate(Vector3.right, rotationAngle, Space.Self);
        _lastPosition = transform.position;
    }

    
    /// <summary>
    /// Initializes the wheel by checking if WheelCollider is null and setting _lastPosition to the current position of the wheel
    /// </summary>
    private void InitializeWheel()
    {
        if (wheelCollider == null)
        {
            enabled = false;
            return;
        }
        _lastPosition = transform.position;
    }
}
