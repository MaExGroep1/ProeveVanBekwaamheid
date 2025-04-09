using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        private EnemyBehaviour _enemy;

        public void CreateEnemy(EnemyData enemyData)
        {
            _enemy = Instantiate(EnemyManager.Instance.EnemyPrefab);
        }
    }
}
