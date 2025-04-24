using System;
using UnityEngine;
using Util;

namespace Car
{
    public class CarData : Singleton<CarData>
    {
        private float _attackSpeedReduction;     //the amount of speed that gets reduced when hitting an enemy, decreases on each upgrade
        private float _defenseFuelDrain;         //the amount of fuel that drains when hitting an enemy, decreases on each upgrade
        private float _fuelContinuousDrain;      //the amount of fuel that continuously drains, decreases on each upgrade
        private float _speed;                    //the speed of the car, increase on each upgrade
        private float _weaponFireRate;           //the speed that the weapon will fire, changes each upgrade based on the weapon


        public float AttackSpeedReduction { get => _attackSpeedReduction; set => _attackSpeedReduction = value; }       //getter/setter for _attackSpeedReduction
        public float DefenseFuelDrain { get => _defenseFuelDrain; set => _defenseFuelDrain = value; }                   //getter/setter for _defenseFuelDrain
        public float FuelContinuousDrain { get => _fuelContinuousDrain; set => _fuelContinuousDrain = value; }          //getter/setter for _fuelContinuousDrain
        public float Speed { get => _speed; set => _speed = value; }                                                    //getter/setter for _speed
        public float WeaponFireRate { get => _weaponFireRate; set => _weaponFireRate = value; }                         //getter/setter for _weaponFireRate
    }
}
