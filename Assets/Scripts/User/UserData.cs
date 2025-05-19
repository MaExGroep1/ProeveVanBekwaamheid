using System;
using Car;
using UnityEngine;
using Util;

namespace User
{
    public class UserData : Singleton<UserData>
    {
        [SerializeField] private CarMovement car;
        [SerializeField] private float unitMultiplier;
        
        private float _startingPosition;

        public float DistanceScore { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _startingPosition = car.transform.position.x;
        }

        private void Update()
        {
            DistanceScore = (car.transform.position.x - _startingPosition) * unitMultiplier;
        }
    }
}
