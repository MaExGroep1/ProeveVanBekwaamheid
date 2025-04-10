using System;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private bool inAir;
        private EnemyBehaviour _enemy;

        private void Start() => EnemyManager.Instance.CreateEnemy(this);

        public bool InAir => inAir;
        public EnemyBehaviour CreateEnemy(EnemyBehaviour enemy)
        {
            _enemy = Instantiate(enemy,transform.position,Quaternion.identity);
            return _enemy;
        }
    }
}
