using System;
using Enemy;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(Rigidbody))]
    public class TestPlayer : MonoBehaviour
    {
        public float speed;
        public float defence;
        public float attack;
        public float fuel;
        public float knockBack;
        public float minimumKnockBack;
        
        private Rigidbody _rigidBody;

        private void Awake() => _rigidBody = GetComponent<Rigidbody>();

        void Update()
        { 
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            if (hor != 0 || ver != 0)
                _rigidBody.AddForce(new Vector3(hor, ver).normalized * speed * _rigidBody.mass);
        }

        public void OnHitEnemy(EnemyBehaviour enemy, Rigidbody enemyRigidbody, float playerForce , float enemyForce , float enemyDefence , float enemyAttack)
        {
            var impact = playerForce + enemyForce;
            var enemyImpact = impact * attack / enemyDefence;
            var playerImpact = impact * enemyAttack / defence;
            var totalKnockBack = playerImpact * knockBack * enemyForce < minimumKnockBack 
                ? playerImpact * knockBack * enemyForce 
                : minimumKnockBack;
            
            fuel -= playerImpact * 0.01f;
            
            _rigidBody.AddForce(new Vector3(-totalKnockBack,0));
            
            enemy.TakeDamage(enemyImpact);

            enemyRigidbody.AddForce(new Vector3(totalKnockBack,0));
        }
    }
}
