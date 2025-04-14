using System;
using UnityEngine;

namespace Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private bool inAir;
        private EnemyBehaviour _enemy;
        
        public bool InAir => inAir;

        private void Start() => EnemyManager.Instance.CreateEnemy(this);
        private void OnDestroy() => EnemyManager.Instance.DestroyEnemy(_enemy);


        private void OnDrawGizmos()
        {
            Gizmos.color = inAir ? Color.cyan: Color.red;
            Gizmos.DrawSphere(transform.position, 1);
        }

        public EnemyBehaviour CreateEnemy(EnemyBehaviour enemy)
        {
            _enemy = Instantiate(enemy,transform.position,Quaternion.identity);
            return _enemy;
        }
    }
}
