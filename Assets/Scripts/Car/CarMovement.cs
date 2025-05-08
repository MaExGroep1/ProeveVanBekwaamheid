using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using Car;
using Grid;
using UnityEngine;
using UnityEngine.Serialization;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private WheelCollider backLeft; // Reference to the Back left wheel
    [SerializeField] private WheelCollider backRight; // Reference to the Back right wheel
    [SerializeField] private float motorForce = 500f; // The Motor Force of the vehicle
    [SerializeField, Range(0, 1)] private float targetThrottle; // The amount of throttle that needs to be multiplied by the motorforce
    [SerializeField] private float _maxFuel = 200f; // The Maximum amount of fuel the car has
    
    private bool _hasMatchedBefore; // Check if the player has matched before so they start with max fuel
    private float _fuel; // The value of the current amount of fuel
    
    private void Awake() =>
        GridManager.Instance.ListenToOnMatch(OnMatch);
    
    private void FixedUpdate() => 
        MoveCar();
    
    /// <summary>
    /// Logic that moves the car forward
    /// </summary>
    private void MoveCar()
    { 
        print(_fuel);
        if (_fuel > 0f)
        {
            ApplyTorque(targetThrottle);

            _fuel -= CarData.Instance.FuelContinuousDrain * Time.deltaTime;
            _fuel = Mathf.Max(_fuel, 0f);
        }
        else ApplyTorque(0f);
    }

    /// <summary>
    /// Applies torque to the wheel collider that allows the car to move forward
    /// </summary>
    /// <param name="throttle"> The Amount to multiply the motorForce by </param>
    private void ApplyTorque(float throttle)
    {
        backLeft.motorTorque = throttle * motorForce;
        backRight.motorTorque = throttle * motorForce;
    }
    
    /// <summary>
    /// Calls when the player makes a match 
    /// </summary>
    /// <param name="blockType"> The type of the block that has been matched </param>
    /// <param name="matchAmount"> The amount of blocks that have been matched </param>
    private void OnMatch(BlockType blockType, int matchAmount)
    {
        if (!IsFirstMatch())
        {
            _fuel += matchAmount;
            _fuel = Mathf.Clamp(_fuel, 0f, _maxFuel);
        }
        else _fuel = _maxFuel;
    }

    /// <summary>
    /// Checks if the player made their first match of the "round"
    /// </summary>
    /// <returns></returns>
    private bool IsFirstMatch()
    {
        if (_hasMatchedBefore)
            return false;

        _hasMatchedBefore = true;
        return true;
    }
    
    /// <summary>
    /// Drains the fuel by a amount
    /// </summary>
    /// <param name="drain"> The amount to drain </param>
    public void DrainFuel(float drain) => _fuel -= drain;
}
