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
    
    [SerializeField, Range(0, 1)] private float targetThrottle = 10f;
    [SerializeField] private float fuelConsumptionRate = 1f;
    
    [SerializeField] private float _maxFuel = 100f;

    private float _fuel;
    


    private void Awake()
    {
        GridManager.Instance.ListenToOnMatch(OnMatch);
    }


    private void FixedUpdate() => MoveCar();
    
    private void MoveCar()
    {
        if (_fuel > 0f)
        {
            ApplyTorque(targetThrottle);

            _fuel -= fuelConsumptionRate * Time.deltaTime;
            _fuel = Mathf.Max(_fuel, 0f);
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

        backLeft.motorTorque = -throttle * motorForce;
        backRight.motorTorque = -throttle * motorForce;
    }
    
    private void OnMatch(BlockType blockType, int matchAmount)
    {
        _fuel += matchAmount; 
        _fuel = Mathf.Clamp(_fuel, 0f, _maxFuel);
        
        print(_fuel);
    }
    
}
