using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{ 
    [Header("Car Variables")]
    [SerializeField] private Transform carTransform; // Reference of the player car transport
    [SerializeField] private Rigidbody carRigidbody; // Reference of the car rigidbody

    [Header("Camera Variables")] 
    [SerializeField] private Transform cameraTransform; // Reference to the camera transform
    [SerializeField] private Vector3 offset = new (1.5f, 0.5f, -1f); // The offset of the camera
    [SerializeField] private float cameraSmoothing; // The time it takes for the camera to move to the target
    [SerializeField] private float bounceIntensity; // The intensity the camera bounces
    
    private Vector3 _velocity = Vector3.zero; // Vector3 zero for use in SmoothDamp

    private void Update()
    {
        // Null check for carTransform and carRigidBody
        if (carTransform == null || carRigidbody == null) 
            return;
        
        SmoothCameraPosition(CalculateBounce());
        LookAtCar();
    }
    
    /// <summary>
    /// Calculates how much the camera bounces
    /// </summary>
    /// <returns>Returns the CameraPosition</returns>
    private Vector3 CalculateBounce()
    {
        var cameraPosition = carTransform.position + offset;
        var bounce = carRigidbody.position.y * bounceIntensity;

        cameraPosition.y += bounce;

        return cameraPosition;
    }
    
    /// <summary>
    /// Makes the camera point at the target
    /// </summary>
    private void LookAtCar()
    {
        cameraTransform.LookAt(carTransform);
    }

    /// <summary>
    /// Makes the camera follow the target with a smooth delay
    /// </summary>
    /// <param name="cameraPos">A Vector 3 of the camera position</param>
    private void SmoothCameraPosition(Vector3 cameraPos)
    {
        var smoothPos = Vector3.SmoothDamp(cameraTransform.position, cameraPos, ref _velocity, cameraSmoothing);
        cameraTransform.position = smoothPos;
    }
}