using Blocks;
using Grid;
using UnityEngine;

namespace Car
{
    public class CarMovement : MonoBehaviour
    {
        [Header("Force")]
        [SerializeField] private WheelCollider[] wheels;                // reference to the wheels
        [SerializeField] private float motorForce = 500f;               // the Motor Force of the vehicle
        [SerializeField, Range(0, 1)] private float targetThrottle;     // the amount of throttle that needs to be multiplied by the motorForce
        [Header("Fuel")]
        [SerializeField] private float maxFuel = 200f;                  // the Maximum amount of fuel the car has
        [SerializeField] private float fuelAddMultiplier;               // the Maximum amount of fuel the car has
    
        private bool _hasMatchedBefore;                                 // check if the player has matched before so they start with max fuel
        private float _fuel;                                            // the value of the current amount of fuel

        public float Fill => _fuel / maxFuel;                           // the percent the fuel tank is full
    
        /// <summary>
        /// Adds listener to on match
        /// </summary>
        private void Awake() =>
            GridManager.Instance.ListenToOnMatch(OnMatch);
    
        private void FixedUpdate() => 
            MoveCar();
    
        /// <summary>
        /// Logic that moves the car forward
        /// </summary>
        private void MoveCar()
        { 
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
            foreach (var wheel in wheels)
                wheel.motorTorque = throttle * motorForce;
        }
    
        /// <summary>
        /// Calls when the player makes a match 
        /// </summary>
        /// <param name="blockType"> The type of the block that has been matched </param>
        /// <param name="matchAmount"> The amount of blocks that have been matched </param>
        private void OnMatch(BlockType blockType, int matchAmount)
        {
            if (_hasMatchedBefore)
            {
                _fuel += matchAmount * fuelAddMultiplier;
                _fuel = Mathf.Clamp(_fuel, 0f, maxFuel);
                return;
            }
            _fuel = maxFuel;
            _hasMatchedBefore = true;
        }
    
        /// <summary>
        /// Drains the fuel by a amount
        /// </summary>
        /// <param name="drain"> The amount to drain </param>
        public void DrainFuel(float drain) => _fuel -= drain;
    }
}
