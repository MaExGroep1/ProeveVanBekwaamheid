using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private int _health;
        private int _damage;
        private int _speed;
        private int _defense;

        public void Initialize(int health, int damage, int speed, int defense)
        {
            _health = health;
            _damage = damage;
            _speed = speed;
            _defense = defense;
        }
    }
}
