using System;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class LaserWeapon : MonoBehaviour
    {
        [SerializeField] private float range;
        [SerializeField] private float damage;
        [SerializeField] private float fireTime;
        [SerializeField] private float fireDelay;
        [SerializeField] private GameObject laserFiringPoint;

        private void Start()
        {
            Debug.DrawRay(laserFiringPoint.transform.position, Vector3.forward * range, Color.red, Mathf.Infinity);
        }

        private void Update()
        {
            if (CanFire()) Shoot();
        }

        private void Shoot()
        {
            if (!Physics.Raycast(laserFiringPoint.transform.position, Vector3.forward, out var hitInfo, range)) return;
            if (!hitInfo.collider.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(damage * Time.deltaTime);
        }

        private bool CanFire()
        {
            //todo Make a timer using fireTime and fireDelay
           return true; 
        }
    }
}
