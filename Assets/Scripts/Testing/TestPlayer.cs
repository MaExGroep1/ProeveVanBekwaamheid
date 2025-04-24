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

            fuel -= playerImpact * 0.01f;
            
            if(enemy.TakeDamage(enemyImpact)) return;
            
            var playerKnockBack = playerImpact * knockBack * enemyForce > minimumKnockBack 
                ? playerImpact * knockBack * enemyForce 
                : minimumKnockBack;
            var enemyKnockBack = enemyImpact * knockBack * playerForce > minimumKnockBack 
                ? enemyImpact * knockBack * playerForce 
                : minimumKnockBack;
            
            _rigidBody.AddForce(new Vector3(-playerKnockBack,0));
            
            enemyRigidbody.AddForce(new Vector3(enemyKnockBack,0));
        }
    }
}
