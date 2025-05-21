using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering;
using Upgrade;

namespace Weapon
{
    public class ProjectileWeapon : MonoBehaviour
    {
        [SerializeField] private float damage;                              //The damage that the projectile will do when it hits an enemy
        [SerializeField] private float projectileTime;                      //The damage that the projectile will do when it hits an enemy
        [SerializeField] private float range;                               //The range in which the weapon tries to find a target and the distance the projectile will fly when no enemies are found
        [SerializeField] private float fireDelay;                           //The time the weapon waits for the next shot after it shoots, this does not include spawn speed and reload speed
        [SerializeField] private float spawnSpeed;                          //The time it takes for a projectile to spawn
        [SerializeField] private LeanTweenType projectileSpawnEaseType;     //The leanTween easing used for spawning the weapon
        [SerializeField] private Transform ammoSpawnPoint;                  //Reference to a gameObject transform that has the location and rotation for spawning the ammunition
        [SerializeField] private WeaponProjectile ammunitionPrefab;         //prefab of the ammunition
        
        private GameObject _target;                                         //The target that will be shot
        
        /// <summary>
        /// Starting the shoot sequence the moment the weapon is instantiated
        /// </summary>
        private void Start()
        {
            AssignEvents();
            ShootSequence();
        }

        /// <summary>
        /// Spawns the ammo and starts the shootDelay
        /// </summary>
        protected void ShootSequence()
        {
            var ammo = SpawnAmmo();
            StartCoroutine(ShootDelay(ammo));
        }
        private void AssignEvents()
        {
            
        }

        /// <summary>
        /// instantiates the ammo on the right place and rotation and tweens the scale from 0 to 1
        /// </summary>
        /// <returns>returns the spawned ammo</returns>
        private WeaponProjectile SpawnAmmo()
        {
            var ammo = Instantiate(ammunitionPrefab, ammoSpawnPoint.position, ammoSpawnPoint.rotation, transform);
            ammo.Initialize(damage, projectileTime, range);
            ammo.transform.localScale = Vector3.zero;
            LeanTween.scale(ammo.gameObject, Vector3.one, spawnSpeed).setEase(projectileSpawnEaseType);
            return ammo;
        }
        
        /// <summary>
        /// Tells the spawned ammo to shoot towards the closest target and resets the shoot loop
        /// </summary>
        /// <param name="ammo">The spawned ammunition</param>
        protected virtual void Shoot(WeaponProjectile ammo)
        {
            ammo.Shoot(GetClosestTarget());
            ShootSequence();
        }

        /// <summary>
        /// Finds the closest enemy within range by using the list of enemies from EnemyManager.enemies
        /// </summary>
        /// <returns>The closest EnemyBehaviour object within range</returns>
        protected EnemyBehaviour GetClosestTarget()
        {
            EnemyBehaviour target = null;
            float closestEnemyDistance = float.MaxValue;
            List<EnemyBehaviour> enemies = new List<EnemyBehaviour>(EnemyManager.Instance.Enemies); 
            
            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance > range) continue;

                if (target != null && closestEnemyDistance < distance) continue;
                target = enemy;
                closestEnemyDistance = distance; 
            }

            return target;
        }


        /// <summary>
        /// Waits until ammo has stopped tweening to avoid issues, then waits for firedelay, the continues the shoot loop
        /// </summary>
        /// <param name="ammo">The spawned ammunition</param>
        /// <returns></returns>
        private IEnumerator ShootDelay(WeaponProjectile ammo)
        {
            while (LeanTween.isTweening(ammo.gameObject))
            {
                yield return null;
            }
            yield return new WaitForSeconds(fireDelay);
            Shoot(ammo);
        }
    }
}
