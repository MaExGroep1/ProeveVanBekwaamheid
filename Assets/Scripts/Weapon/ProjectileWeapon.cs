using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.Rendering;

namespace Weapon
{
    public class ProjectileWeapon : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float range;
        [SerializeField] private float fireDelay;
        [SerializeField] private float spawnSpeed;
        [SerializeField] private LeanTweenType projectileEaseType;
        [SerializeField] private GameObject ammoSpawnPoint;
        [SerializeField] private WeaponProjectile ammunitionPrefab;
        
        private GameObject _target;
        
        private void Start()
        {
            ShootSequence();
        }

        protected void ShootSequence()
        {
            var ammo = SpawnAmmo();
            StartCoroutine(ShootDelay(ammo));
        }

        private WeaponProjectile SpawnAmmo()
        {
            var ammo = Instantiate(ammunitionPrefab, ammoSpawnPoint.transform.position, ammoSpawnPoint.transform.rotation);
            ammo.Initialize(damage, projectileSpeed, range);
            ammo.transform.localScale = Vector3.zero;
            LeanTween.scale(ammo.gameObject, Vector3.one, spawnSpeed).setEase(projectileEaseType);
            return ammo;
        }

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

        protected virtual void Shoot(WeaponProjectile ammo)
        {
            ammo.Shoot(GetClosestTarget());
            ShootSequence();
        }

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
