using System;
using Enemy;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public float speed;
        public float defence;
        public float attack;
        public float fuel;
        
        private Rigidbody _rigidBody;

        private void Awake() => _rigidBody = GetComponent<Rigidbody>();

        void Update()
        { 
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            if (hor != 0 || ver != 0)
                _rigidBody.AddForce(new Vector3(hor, ver).normalized * speed * _rigidBody.mass);
        }

        public void OnHitEnemy(EnemyBehaviour enemy, Rigidbody enemyRigidbody, float impact , float enemyDefence , float enemyAttack)
        {
            var enemyImpact = impact * attack / enemyDefence;
            var playerImpact = impact * enemyAttack / defence;
            
            Debug.Log($"Enemy {enemyImpact}, Player {playerImpact}");
            
            fuel -= playerImpact;
            
            _rigidBody.AddForce(new Vector3(-playerImpact * 5,0));
            
            enemy.TakeDamage(enemyImpact);

            if (GetComponent<Collider>() == null) return;
            enemyRigidbody.AddForce(new Vector3(enemyImpact * 5,0));
        }
    }
}
