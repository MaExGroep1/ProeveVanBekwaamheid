using UnityEngine;

namespace Car
{
    public class CarFlip : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;   // the rigid body of this gameObject
        [SerializeField] private float multiplier;      // the amount of strength of the flip

        private void Update() => CalculateCenterOfMass();

        /// <summary>
        /// Calculates the center of mass of the car to flip it around
        /// </summary>
        private void CalculateCenterOfMass()
        {
            var rotation = transform.rotation.eulerAngles.x;
            rotation = rotation switch
            {
                > 0 and < 180 => -transform.rotation.eulerAngles.x,
                > 180 => transform.rotation.eulerAngles.x - 360,
                < 0 and > -180 => transform.rotation.eulerAngles.x,
                < -180 => -transform.rotation.eulerAngles.x - 360,
                _ => rotation
            };

            rigidBody.centerOfMass = new Vector3(0, rotation * multiplier, 0);
        }
    }
}
