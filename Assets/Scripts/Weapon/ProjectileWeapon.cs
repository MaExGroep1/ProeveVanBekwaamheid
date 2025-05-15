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
            while (!LeanTween.isTweening(ammo.gameObject))  break;
            StartCoroutine(ShootDelay(ammo));
        }

        private WeaponProjectile SpawnAmmo()
        {
            var ammo = Instantiate(ammunitionPrefab, ammoSpawnPoint.transform.position, ammoSpawnPoint.transform.rotation);
            ammo.Initialize(damage, projectileSpeed);
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

                if (target != null && closestEnemyDistance < distance) continue;
                target = enemy;
                closestEnemyDistance = distance; 
            }

            return target;
        }

        protected virtual void Shoot(WeaponProjectile ammo)
        {
            ammo.OnDestroyed += ShootSequence;
            ammo.Shoot(GetClosestTarget());
        }

        private IEnumerator ShootDelay(WeaponProjectile ammo)
        {
            yield return new WaitForSeconds(fireDelay);
            Shoot(ammo);
        }
    }
}
