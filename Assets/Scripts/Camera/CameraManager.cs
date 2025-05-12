using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{ 
    [Header("Car Variables")]
    [SerializeField] private Transform carTransform; // Reference of the player car transport
    [SerializeField] private Rigidbody carRigidbody; // Reference of the car rigidbody

    [Header("Camera Variables")] 
    [SerializeField] private Transform cameraTransform; // Reference to the camera transform
    [SerializeField] private Vector3 cameraOffset = new (1.5f, 0.5f, 0); // The offset of the camera

    private void LateUpdate()
    {
        // Null check for carTransform and carRigidBody
        if (carTransform == null || carRigidbody == null) 
            return;
        
        CameraPosition();
    }


    /// <summary>
    /// Camera follows the carTransform
    /// </summary>
    private void CameraPosition()
    {
        cameraTransform.position = carTransform.position + cameraOffset;
    }
}