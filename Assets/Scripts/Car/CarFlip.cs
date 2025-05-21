using UnityEngine;

namespace Car
{
    public class CarFlip : MonoBehaviour
    {
        [SerializeField] private Rigidbody rigidBody;   // the rigid body of this gameObject
        [SerializeField] private float multiplier;      // the amount of strength of the flip

        private void Update() => CalculateCenterOfMass();

        /// <summary>
        /// calculates the center of mass of the car to flip it around
        /// </summary>
        private void CalculateCenterOfMass()
        {
            var rotation = -180 + Mathf.Abs(Mathf.Abs(transform.rotation.eulerAngles.x) - Mathf.Abs(180));
            rigidBody.centerOfMass = new Vector3(0, rotation * multiplier, 0);
        }
    }
}
