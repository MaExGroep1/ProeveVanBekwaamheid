using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider backLeft;
    [SerializeField] private WheelCollider backRight;
    [SerializeField] private float motorForce = 1500f;

    private void Update()
    {
        float throttle = Input.GetAxis("Horizontal");
        
        ApplyTorque(throttle);
    }

    private void ApplyTorque(float throttle)
    {
        backLeft.motorTorque = throttle * motorForce;
        backRight.motorTorque = throttle * motorForce;

    }
}
