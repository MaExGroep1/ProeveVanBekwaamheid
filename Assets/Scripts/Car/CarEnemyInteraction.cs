using System;
using Enemy;
using Sound;
using UnityEngine;

namespace Car
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CarMovement))]
    public class CarEnemyInteraction : MonoBehaviour
    {
        [SerializeField] private float knockBack;                       // the knock back that the car has
        [SerializeField] private float minimumKnockBack;                // the minimum knock back of the impact
        [SerializeField] private float airForceMultiplier;              // the minimum knock back of the impact
        [SerializeField] private float minimumEnemyDamage;              // the minimum knock back of the impact
        [SerializeField] private float playerKnockBackMultiplier;       // the minimum knock back of the impact
        [SerializeField] private SoundService audioSource;              //Sound service for playing sound clips

        
        private Rigidbody _rigidBody;       // the rigid body of the car
        private CarMovement _carMovement;   // the movement script of the car


        private void Awake() => SetVariables();
        
        /// <summary>
        /// Adds force to the player and enemy based on the impact
        /// </summary>
        /// <param name="enemy"> The enemy behaviour </param>
        /// <param name="enemyRigidbody"> The enemy rigidbody </param>
        /// <param name="playerForce"> The force of the player </param>
        /// <param name="enemyForce"> The force of the enemy </param>
        /// <param name="enemyDefence"> The enemy defence </param>
        /// <param name="enemyAttack"> The enemy attack </param>
        public void OnHitEnemy(EnemyBehaviour enemy, Rigidbody enemyRigidbody, float playerForce , float enemyForce , float enemyDefence , float enemyAttack)
        {
            audioSource.PlaySound();
            
            var impact = playerForce + enemyForce;
            
            var enemyImpact = impact * CarData.Instance.AttackSpeedReduction / enemyDefence;
            var playerImpact = impact * enemyAttack / CarData.Instance.DefenseFuelDrain;

            var enemyDamage = enemyImpact > minimumEnemyDamage ? enemyImpact : minimumEnemyDamage;
            
            var totalKnockBack = playerImpact * knockBack * enemyForce < minimumKnockBack 
                ? playerImpact * knockBack * enemyForce 
                : minimumKnockBack;
            
            _carMovement.DrainFuel(playerImpact * 0.01f);
            
            enemy.TakeDamage(enemyDamage/3);

            if (enemy.MarkedForDeletion) return;
            
            _rigidBody.AddForce(new Vector3(-totalKnockBack* playerKnockBackMultiplier *_rigidBody.mass,0));
            
            enemyRigidbody.AddForce(new Vector3(totalKnockBack,totalKnockBack * airForceMultiplier));
        }

        /// <summary>
        /// Sets the movement scripts and the rigidbody
        /// </summary>
        private void SetVariables()
        {
            _carMovement = GetComponent<CarMovement>();
            _rigidBody = GetComponent<Rigidbody>();
        }
    }
}
