using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace Weapon
{
    public class ProjectileWeapon : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float fireDelay;
        [SerializeField] private GameObject ammoSpawnPoint;
        [SerializeField] private WeaponProjectile ammunitionPrefab;
        
        private GameObject _target;

        private void Start()
        {
            StartCoroutine(Shoot());
        }

        private WeaponProjectile SpawnAmmo()
        {
            var ammo = Instantiate(ammunitionPrefab, ammoSpawnPoint.transform.position, ammoSpawnPoint.transform.rotation);
            return ammo;
        }

        private EnemyBehaviour GetClosestTarget()
        {
            EnemyBehaviour target = null;
            float closestEnemyDistance = float.MaxValue;
            List<EnemyBehaviour> enemies = new List<EnemyBehaviour>(EnemyManager.Instance.Enemies); 
            foreach (var enemy in enemies)
            {
                var distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (target != null && closestEnemyDistance < distance) continue;
                target = enemy;
                closestEnemyDistance = distance; 
            }
            Debug.Log("closest target = " + target);
            return target;
        }

        private IEnumerator Shoot()
        {
            yield return new WaitForSeconds(fireDelay);
            var ammo = SpawnAmmo();
            ammo.Shoot(GetClosestTarget(), damage, projectileSpeed);
            StartCoroutine(Shoot());
        }
    }
}
