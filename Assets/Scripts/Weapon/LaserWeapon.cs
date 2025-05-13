using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class LaserWeapon : MonoBehaviour
    {
        [SerializeField] private float damage;
        [SerializeField] private float fireTime;
        [SerializeField] private float fireDelay;
        [SerializeField] private GameObject laser;
        [SerializeField] private LayerMask laserLayerMask;
        
        private bool _canFire;
        private void Update()
        {
            if (_canFire) Shoot();
        }

        private void Shoot()
        {
            var laserBeam = laser;
            var rayDirection = laserBeam.transform.right;
            
            if (!Physics.Raycast(laserBeam.transform.position, rayDirection, out var hitInfo, Mathf.Infinity))
            {
                ScaleLaser(Vector3.zero);
                return;
            }
            ScaleLaser(hitInfo.point);
            if (!hitInfo.collider.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(damage * Time.deltaTime);
        }

        private void ScaleLaser(Vector3 hitPoint)
        {
            var laserBeam = laser;
            Vector3 scale = laserBeam.transform.localScale;
            float distance = hitPoint != Vector3.zero ? Vector3.Distance(hitPoint, laserBeam.transform.position) : 0f;

            scale.x = distance;
            laserBeam.transform.localScale = scale;
        }

        private bool CanFire()
        {
           return true;
           return false;
        }
    }
}
