using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData", order = 1)]
    public class EnemyData : ScriptableObject
    {
        [Header("Stats")]
        public int health;
        public int damage;
        public int speed;
        public int defense;
        
        [Header("Visuals")]
        public GameObject prefab;
        public float scale;
    }
}
