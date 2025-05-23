using System;
using Car;
using UnityEngine;
using Util;

namespace User
{
    public class UserData : Singleton<UserData>
    {
        [SerializeField] private CarMovement car;           // the car to track
        [SerializeField] private float unitMultiplier;      // how much distance to traverse for every unit
        
        private float _startingPosition;                    // the position the car started at

        public float DistanceScore { get; private set; }    // the current score gained from distance

        /// <summary>
        /// Sets the starting position
        /// </summary>
        private void Start() => 
            _startingPosition = car.transform.position.x;
        
        /// <summary>
        /// Sets the DistanceScore to the correct amount
        /// </summary>
        private void Update() =>
            DistanceScore = (car.transform.position.x - _startingPosition) * unitMultiplier;
        
    }
}
