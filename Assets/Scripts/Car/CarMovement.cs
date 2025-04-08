using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider backLeft; // Reference to the Back left wheel
    [SerializeField] private WheelCollider backRight; // Reference to the Back right wheel
    [SerializeField] private float motorForce = 1500f; // The Motor Force of the vehicle
    [SerializeField] private float brakeForce = 3000f; // The Brake Force of the vehicle

    private void Update()
    {
        float throttle = Input.GetAxis("Vertical");

        if (Mathf.Abs(throttle) > 0.01f)
        {
            ApplyTorque(throttle);
            ApplyBrake(0f);
        }
        else
        {
            ApplyTorque(0f);
            ApplyBrake(brakeForce);
        }

    }

    /// <summary>
    /// Applies a brakeforce to slow down and stop the vehicle
    /// </summary>
    /// <param name="brakeForce">The Amount the vehicle brakes</param>
    
    private void ApplyBrake(float brakeForce)
    {
        backLeft.brakeTorque = brakeForce;
        backRight.brakeTorque = brakeForce;
    }

    /// <summary>
    /// Applies torque to the wheel collider that allows the car to move forward
    /// </summary>
    /// <param name="throttle">The Amount to multiply the motorForce by</param>

    private void ApplyTorque(float throttle)
    {
        backLeft.motorTorque = throttle * motorForce;
        backRight.motorTorque = throttle * motorForce;
    }
}
