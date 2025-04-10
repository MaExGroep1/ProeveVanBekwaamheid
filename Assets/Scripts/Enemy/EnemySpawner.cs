using System;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private bool inAir;
        private EnemyBehaviour _enemy;
        
        private void Awake() => EnemyManager.Instance.CreateEnemy(this);

        public bool InAir => inAir;
        public EnemyBehaviour CreateEnemy(EnemyData enemyData,EnemyBehaviour enemy)
        {
            _enemy = Instantiate(enemy,transform.position,Quaternion.identity);
            _enemy.Initialize(enemyData);
            return _enemy;
        }
    }
}
