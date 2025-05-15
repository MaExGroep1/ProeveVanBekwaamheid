using System;
using System.Collections;
using Interfaces;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapon
{
    public class LaserWeapon : MonoBehaviour
    {
        [SerializeField] private float damage;      //the damage the laser does when hitting an enemy per frame * deltatime
        [SerializeField] private float fireTime;    //the time the laser will fire before turning off
        [SerializeField] private float fireDelay;   //the time the laser is turned off before firing
        [SerializeField] private float baseRange;   //the base range that the laser visual has
        [SerializeField] private GameObject laser;  //the ref to the laser visual object
        
        private bool _canFire;                      //fires the laser on true and disables on false

        private bool CanFire                        //getter/setter for _canFire, when changed will reset the FireTimer and when turned false will disable the laser visual
        {
            get => _canFire;
            set
            {
                _canFire = value;
                StartCoroutine(FireTimer());
                if (value == false) ScaleLaser(Vector3.zero); 
            }
        }

        /// <summary>
        /// starts the FireTimer loop
        /// </summary>
        private void Start()
        {
            StartCoroutine(FireTimer());
        }
        
        /// <summary>
        /// Shoots the laser when CanFire is true
        /// </summary>
        private void Update()
        {
            if (CanFire) Shoot();
        }

        /// <summary>
        /// casts a raycast forward and checks to see if it hit an enemy, will dela damage when an enemy is found, then scales the laser visual to the hitpoint
        /// </summary>
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

        /// <summary>
        /// Scales the visual laser to "hitPoint" location, will disable it when hitPoints is 0,0,0
        /// </summary>
        /// <param name="hitPoint"></param>
        private void ScaleLaser(Vector3 hitPoint)
        {
            var laserBeam = laser;
            Vector3 scale = laserBeam.transform.localScale;
            float distance = hitPoint != Vector3.zero ? Vector3.Distance(hitPoint, laserBeam.transform.position) : baseRange;

            scale.x = distance; 
            laserBeam.transform.localScale = scale;
        }

        /// <summary>
        /// will wait a determined time and swaps CanFire bool
        /// will wait for FireTime amount when canFire is true and will wait fireDelay amount when canFire is false
        /// </summary>
        /// <returns></returns>
        private IEnumerator FireTimer()
        {
            var canFire = CanFire;
            var timer = canFire ? fireTime : fireDelay;
            yield return new WaitForSeconds(timer);
            CanFire = !canFire;
        }
    }
}
