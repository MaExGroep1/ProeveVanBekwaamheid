using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using Grid;
using UnityEngine;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider backLeft; // Reference to the Back left wheel
    [SerializeField] private WheelCollider backRight; // Reference to the Back right wheel
    [SerializeField] private float motorForce = 1500f; // The Motor Force of the vehicle
    [SerializeField] private float brakeForce = 3000f; // The Brake Force of the vehicle
    
    [SerializeField, Range(0, 1)] private float targetThrottle = 1f;
    [SerializeField] private float fuelConsumptionRate = 5f;

    private float _fuel;
    private float _maxFuel;


    private void Awake()
    {
        GridManager.Instance.ListenToOnMatch(OnMatch);
    }


    private void Update()
    {
        if (_fuel > 0f)
        {
            
        }
        else
        {
            
        }
    }

    /// <summary>
    /// Applies a brakeforce to slow down and stop the vehicle
    /// </summary>
    /// <param name="brakeForce">The Amount the vehicle brakes</param>
    
    private void ApplyBrake(float throttle)
    {
        var fuelFactor = Mathf.Clamp01(_fuel / _maxFuel);
        var adjustedForce = throttle * motorForce * fuelFactor;

        backLeft.motorTorque = adjustedForce;
        backRight.motorTorque = adjustedForce;
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

    
    private void OnMatch(BlockType blockType, int matchAmount)
    {
        _fuel = Mathf.Clamp(_fuel, 0f, _maxFuel);
        _fuel += matchAmount; 
    }
    
}
