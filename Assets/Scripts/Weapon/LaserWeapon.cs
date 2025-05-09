using System;
using Interfaces;
using UnityEngine;

namespace Weapon
{
    public class LaserWeapon : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private float fireTime;
        [SerializeField] private float fireDelay;
        [SerializeField] private GameObject laserFiringPoint;

        private void Update()
        {
            if (CanFire()) Shoot();
            
        }

        private void Shoot()
        {
            if (!Physics.Raycast(laserFiringPoint.transform.position, Vector3.right,  out var hitInfo)) return;
            if (!hitInfo.collider.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(damage * Time.deltaTime);
            Debug.Log("TakesDamage");
        }

        private bool CanFire()
        {
            //todo Make a timer using fireTime and fireDelay
           return true; 
        }
    }
}
