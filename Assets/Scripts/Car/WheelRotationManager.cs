using System;
using System.Collections;
using System.Collections.Generic;
using Blocks;
using UnityEngine;
using Upgrade;

public class WheelRotationManager : MonoBehaviour
{
    private bool _isRotating = true;
    
    [SerializeField] private WheelCollider wheelCollider; // Reference to the WheelCollider

    [SerializeField] private Transform[] wheelTransforms;
    
    private Vector3 _lastPosition; // The last position of the wheel

    private void Start()
    {
        AssignEvents();
        InitializeWheel();
    }
    
    private void Update()
    {
        if (_isRotating) RotateWheels();
    }
    private void AssignEvents()
    {
        if (!UpgradeManager.Instance.OnUpgradeCompleted.TryAdd(BlockType.Speed, EnableWheelRotating)) UpgradeManager.Instance.OnUpgradeCompleted[BlockType.Speed] += EnableWheelRotating;
        if (!UpgradeManager.Instance.OnUpgrade.TryAdd(BlockType.Speed, DisableWheelRotating)) UpgradeManager.Instance.OnUpgrade[BlockType.Speed] += DisableWheelRotating;
    }

    private void EnableWheelRotating() => _isRotating = true;
    private void DisableWheelRotating() => _isRotating = false;
    
    
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
