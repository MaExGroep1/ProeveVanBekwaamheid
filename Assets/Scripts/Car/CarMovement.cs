using System.Collections;
using Blocks;
using Grid;
using UI.PopUp;
using UnityEngine;
using Util;

namespace Car
{
    public class CarMovement : MonoBehaviour
    {
        [Header("Force")]
        [SerializeField] private WheelCollider[] wheels;                // reference to the wheels
        [SerializeField] private float motorForce = 500f;               // the Motor Force of the vehicle
        [SerializeField, Range(0, 1)] private float targetThrottle;     // the amount of throttle that needs to be multiplied by the motorForce
        [SerializeField] private AnimationCurve torqueCurve;            // the acceleration of the car
        [SerializeField] private float maxSpeed = 60f;                  // the cars maximum speed
        [Header("Fuel")]
        [SerializeField] private float maxFuel = 200f;                  // the Maximum amount of fuel the car has
        [SerializeField] private float fuelAddMultiplier;               // the Maximum amount of fuel the car has
        [Header("Tilt")]
        [SerializeField] private Rigidbody mainBody;                    // the main body of the car
        [SerializeField] private float tiltMultiplier;                  // the multiplier of the extra throttle while the car is tilted
        [SerializeField] private MinMax<float> tiltClamp;               // the minimum and maximum throttle tilt addition
        [Header("GameOverPopUp")]
        [SerializeField] private PopUp gameOverPopUp;                   // the game over prefab to instantiate
        [SerializeField] private float carRollTime;                     // the time to wait after having dropped to 0 fuel to create the game over pop up
        
        private float _fuel;                                            // the value of the current amount of fuel
        private bool _hasHadZeroFuel = true;                            // whether the car has been at 0 fuel

        public float Fill => _fuel / maxFuel;                           // the percent the fuel tank is full
        
        private void Awake() => AddListeners();
    
        
        private void FixedUpdate() => MoveCar();

        /// <summary>
        /// Adds listener to on match
        /// </summary>
        private void AddListeners()
        {
            GridManager.Instance.ListenToOnMatch(OnMatch);
            GridManager.Instance.ListenToOnFirstMatch(OnFirstMatch);
        }
        
        /// <summary>
        /// Logic that moves the car forward
        /// </summary>
        private void MoveCar()
        { 
            if (_fuel > 0f)
            {
                ApplyTorque(targetThrottle);

                DrainFuel(CarData.Instance.FuelContinuousDrain * Time.deltaTime);
            }
            else ApplyTorque(0f);
        }

        /// <summary>
        /// Applies torque to the wheel collider that allows the car to move forward
        /// </summary>
        /// <param name="throttle"> The Amount to multiply the motorForce by </param>
        private void ApplyTorque(float throttle)
        {
            var speedFactor = mainBody.velocity.magnitude / maxSpeed;
            var torqueMultiplier = torqueCurve.Evaluate(speedFactor);
            var rotation = mainBody.transform.rotation.eulerAngles.x switch
            {
                > 0 and < 180 => -mainBody.transform.rotation.eulerAngles.x,
                > 180 => mainBody.transform.rotation.eulerAngles.x - 360,
                < 0 and > -180 => mainBody.transform.rotation.eulerAngles.x,
                < -180 => -mainBody.transform.rotation.eulerAngles.x - 360,
                _ => mainBody.transform.rotation.eulerAngles.x
            };
            
            var tiltForce = Mathf.Clamp(-rotation, tiltClamp.min, tiltClamp.max) * tiltMultiplier;
            
            Debug.Log(tiltForce);
            var force = throttle * motorForce * tiltForce * torqueMultiplier;
            
            foreach (var wheel in wheels)
                wheel.motorTorque = force;
        }
        
        /// <summary>
        /// Calls when the player makes a match 
        /// </summary>
        /// <param name="blockType"> The type of the block that has been matched </param>
        /// <param name="matchAmount"> The amount of blocks that have been matched </param>
        private void OnMatch(BlockType blockType, int matchAmount)
        {
            if (_hasHadZeroFuel) return;
            _fuel += matchAmount * fuelAddMultiplier;
            _fuel = Mathf.Clamp(_fuel, 0f, maxFuel);
        }

        /// <summary>
        /// Sets the fuel to the maximum capacity
        /// </summary>
        private void OnFirstMatch()
        {
            _hasHadZeroFuel = false;
            _fuel = maxFuel;
        }

        /// <summary>
        /// Drains the fuel by an amount
        /// </summary>
        /// <param name="drain"> The amount to drain </param>
        public void DrainFuel(float drain)
        {
            _fuel -= drain;
            if (_fuel > 0 || _hasHadZeroFuel) return;
            _hasHadZeroFuel = true;
            StartCoroutine(WaitToCreatePopUp());
        }

        /// <summary>
        /// Waits a amount of time then makes the game over pop up
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitToCreatePopUp()
        {
            yield return new WaitForSeconds(carRollTime);
            Instantiate(gameOverPopUp);
        }
    }
}
