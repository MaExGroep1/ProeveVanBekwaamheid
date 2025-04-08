using Util;

namespace Car
{
    public class CarData : Singleton<CarData>
    {
        private float _attackSpeedReduction;     //the amount of speed that gets reduced when hitting an enemy, decreases on each upgrade
        private float _defenseFuelDrain;         //the amount of fuel that drains when hitting an enemy, decreases on each upgrade
        private float _fuelContinuousDrain;      //the amount of fuel that continuously drains, decreases on each upgrade
        private float _speed;                    //the speed of the car, increase on each upgrade

        public float AttackSpeedReduction { get => _attackSpeedReduction; set => _attackSpeedReduction = value; }
        public float DefenseFuelDrain { get => _defenseFuelDrain; set => _defenseFuelDrain = value; }
        public float FuelContinuousDrain { get => _fuelContinuousDrain; set => _fuelContinuousDrain = value; }
        public float Speed { get => _speed; set => _speed = value; }
    }
}
