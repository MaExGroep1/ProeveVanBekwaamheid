using System;
using System.Diagnostics;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private bool inAir;    // whether the spawner is in the air
        private EnemyBehaviour _enemy;          // this spawners enemy
        
        public bool InAir => inAir;

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
            EnemyManager.Instance.DestroyEnemy(_enemy);
        }

        /// <summary>
        /// Shows whether the spawner is in the air or on the ground
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = inAir ? Color.cyan: Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }
        
        /// <summary>
        /// Creates a new enemy
        /// </summary>
        /// <param name="enemy"> new enemy prefab </param>
        /// <returns> the new enemy </returns>
        public EnemyBehaviour CreateEnemy(EnemyBehaviour enemy)
        {
            _enemy = Instantiate(enemy,transform.position,Quaternion.identity);
            _enemy.StartRoam(transform);
            return _enemy;
        }
    }
}
