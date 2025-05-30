using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using Car;
using UnityEngine;
using Upgrade;
using Upgrade.UpgradeParts;

public class WheelRotationManager : MonoBehaviour
{
    [SerializeField] private WheelCollider wheelCollider;           // Reference to the WheelCollider
    [SerializeField] private Transform[] wheelTransforms;           // References to the wheel transforms
    [SerializeField] private int rotationStopUpgradeLevel;          // The upgrade level where rotation will be disabled
    [SerializeField] private SpeedUpgradeBehaviour speedUpgrades;   // The speed upgrade, used to check when to disable rotation for the upgrades
    
    private Vector3 _lastPosition;                                  // The last position of the wheel
    private bool _isRotating = true;                                // Bool to enable and disable the wheel rotation
    private List<Vector3> _wheelRotations = new List<Vector3>();    // Stores the base rotations of the wheels to be able to reset it
    
    /// <summary>
    /// assigns events and Initializes
    /// </summary>
    private void Start()
    {
        AssignEvents();
        GetBaseWheelRotations();
        InitializeWheel();
    }


    /// <summary>
    /// Will rotate wheels if _isRotating is ture
    /// </summary>
    private void Update()
    {
        if (_isRotating) RotateWheels();
    }
    
    private void GetBaseWheelRotations()
    {
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            _wheelRotations.Add(wheelTransforms[i].localRotation.eulerAngles);
        }
    }
    
    /// <summary>
    /// assigns events
    /// </summary>
    private void AssignEvents()
    {
        speedUpgrades.OnUpgradeComplete += EnableWheelRotating; 
        speedUpgrades.OnUpgrade += DisableWheelRotating;
        speedUpgrades.OnHoverWheels += RotationPermaStop;
        
    }

    /// <summary>
    /// unassigns all events
    /// </summary>
    private void UnassignEvents()
    {
        speedUpgrades.OnUpgradeComplete -= EnableWheelRotating; 
        speedUpgrades.OnUpgrade -= DisableWheelRotating;
        speedUpgrades.OnHoverWheels -= RotationPermaStop;
    }
    
    /// <summary>
    /// Permanently stops the wheel rotation and sets the wheels to rotation 0, for when wheels are aquired that do not rotate
    /// </summary>
    private void RotationPermaStop()
    {
        DisableWheelRotating();
        UnassignEvents();
        for (int i = 0; i < wheelTransforms.Length; i++)
        {
            wheelTransforms[i].localEulerAngles = _wheelRotations[i];
        } 
        enabled = false;
    }


    
    private void EnableWheelRotating() => _isRotating = true;   //sets isRotating to true (used for events)
    
    private void DisableWheelRotating() => _isRotating = false; //sets isRotating to true (used for events)
    
    
    /// <summary>
    /// Rotates the wheel based on how fast it has moved
    /// </summary>
    private void RotateWheels()
    {
        var movement = transform.position - _lastPosition;

        var distance = Vector3.Dot(movement, transform.parent.forward);
        var rotationAngle = (distance / (2 * Mathf.PI * wheelCollider.radius)) * 360f;

        for (int i = 0; i < wheelTransforms.Length; i++)
            wheelTransforms[i].Rotate(Vector3.right, -rotationAngle, Space.Self);
        
        _lastPosition = transform.position;
    }

    
    /// <summary>
    /// Initializes the wheel by checking if WheelCollider is null and setting _lastPosition to the current position of the wheel
    /// </summary>
    private void InitializeWheel()
    {
        if (wheelCollider == null || wheelTransforms == null)
        {
            enabled = false;
            return;
        }
        _lastPosition = transform.position;
    }
}
