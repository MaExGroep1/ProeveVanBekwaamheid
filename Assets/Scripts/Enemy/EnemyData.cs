using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        public int health;
        public int damage;
        public int speed;
        public int defense;
    }
}
