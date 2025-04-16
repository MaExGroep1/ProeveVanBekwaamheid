using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{ 
    [Header("Car Variables")]
    [SerializeField] private Transform carTransform;
    [SerializeField] private Rigidbody carRigidbody;

    [Header("Camera Variables")] 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Vector3 offset = new (1.5f, 0.5f, -1f);
    [SerializeField] private float cameraSmoothing;
    [SerializeField] private float bounceIntensity;
    
    private Vector3 _velocity = Vector3.zero;

    private void Update()
    {
        var cameraPosition = carTransform.position + offset;

        var bounce = carRigidbody.position.y * bounceIntensity;

        cameraPosition.y += bounce;

        var smoothPos = Vector3.SmoothDamp(cameraTransform.position, cameraPosition, ref _velocity, cameraSmoothing);

        cameraTransform.position = smoothPos;

        cameraTransform.LookAt(carTransform);
    }
}