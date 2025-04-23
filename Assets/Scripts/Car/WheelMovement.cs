using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider wheelCollider;
    
    private Vector3 _lastPosition;

    private void Start() => 
        _lastPosition = transform.position;

    private void Update() => RotateWheel();

    private void RotateWheel()
    {
        var movement = transform.position - _lastPosition;

        var distance = Vector3.Dot(movement, transform.parent.forward);
        var rotationAngle = (distance / (2 * Mathf.PI * wheelCollider.radius)) * 360f;
        
        transform.Rotate(Vector3.right, rotationAngle, Space.Self);
        _lastPosition = transform.position;
    }

    private void NullCheck()
    {
        if (wheelCollider == null)
        {
            enabled = false;
            return;
        }
    }
}
