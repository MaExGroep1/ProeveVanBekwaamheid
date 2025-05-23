using Tiles;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [field: SerializeField] public bool InAir { get; private set; }     // whether the spawner is in the air
        private EnemyBehaviour _enemy;                                      // this spawners enemy
        

        /// <summary>
        /// Spawns an enemy
        /// </summary>
        private void Start() => EnemyManager.Instance.CreateEnemy(this);
        
        
        /// <summary>
        /// Destroys its own enemy
        /// </summary>
        private void OnDestroy()
        {
            if (_enemy == null) return;
            EnemyManager.Instance.DestroyEnemy(_enemy, false);
        }

        /// <summary>
        /// Shows whether the spawner is in the air or on the ground
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = InAir ? Color.cyan: Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
        
        /// <summary>
        /// Creates a new enemy
        /// </summary>
        /// <param name="enemy"> new enemy prefab </param>
        /// <returns> the new enemy </returns>
        public EnemyBehaviour CreateEnemy(EnemyBehaviour enemy)
        {
            // ReSharper disable once PossibleLossOfFraction
            var amount = TileManager.Instance.TileAmount / EnemyManager.Instance.EnemyMultiplierAmount;
            _enemy = Instantiate(enemy,transform.position,Quaternion.identity);
            _enemy.ApplyMultiplier(1+amount);
            _enemy.StartRoam(transform);
            return _enemy;
        }
    }
}
