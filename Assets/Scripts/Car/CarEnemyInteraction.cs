using System;
using Enemy;
using UnityEngine;

namespace Car
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CarMovement))]
    public class CarEnemyInteraction : MonoBehaviour
    {
        public float knockBack;             // the knock back that the car has
        public float minimumKnockBack;      // the minimum knock back of the impact
        
        private Rigidbody _rigidBody;       // the rigid body of the car
        private CarMovement _carMovement;   // the movement script of the car


        private void Awake() => SetVariables();

        /// <summary>
        /// Sets the movement scripts and the rigidbody
        /// </summary>
        private void SetVariables()
        {
            _carMovement = GetComponent<CarMovement>();
            _rigidBody = GetComponent<Rigidbody>();
        }
        
        /// <summary>
        /// Adds force to the player and enemy based on the impact
        /// </summary>
        /// <param name="enemy"> The enemy behaviour </param>
        /// <param name="enemyRigidbody"> The enemy rigidbody </param>
        /// <param name="playerForce"> the force of the player </param>
        /// <param name="enemyForce"> the force of the enemy </param>
        /// <param name="enemyDefence"> the enemy defence </param>
        /// <param name="enemyAttack"> the enemy attack </param>
        public void OnHitEnemy(EnemyBehaviour enemy, Rigidbody enemyRigidbody, float playerForce , float enemyForce , float enemyDefence , float enemyAttack)
        {
            var impact = playerForce + enemyForce;
            var enemyImpact = impact * CarData.Instance.AttackSpeedReduction / enemyDefence;
            var playerImpact = impact * enemyAttack / CarData.Instance.DefenseFuelDrain;
            var totalKnockBack = playerImpact * knockBack * enemyForce < minimumKnockBack 
                ? playerImpact * knockBack * enemyForce 
                : minimumKnockBack;
            
            _carMovement.DrainFuel(playerImpact * 0.01f);
            
            _rigidBody.AddForce(new Vector3(-totalKnockBack,0));
            
            enemy.TakeDamage(enemyImpact);

            enemyRigidbody.AddForce(new Vector3(totalKnockBack,0));
        }
    }
}
