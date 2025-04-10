using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        private int _health;
        private int _damage;
        private int _speed;
        private int _defense;

        public void Initialize(EnemyData enemyData)
        {
            var enemyGameObject = Instantiate(enemyData.prefab, transform.position, Quaternion.identity);
            
            _health = enemyData.health;
            _damage = enemyData.damage;
            _speed = enemyData.speed;
            _defense = enemyData.defense;
            transform.localScale = Vector3.one * enemyData.scale;
            
            enemyGameObject.transform.parent = transform;
        }
    }
}
