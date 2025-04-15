using System;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{ 
    [SerializeField] private Transform camera;
    [SerializeField] private Transform target;
    [SerializeField] private float yAxis, zAxis;

    private void Update()
    {
        camera.position = new Vector3(target.position.x, target.transform.position.y + yAxis, target.transform.position.z + zAxis);
    }
}