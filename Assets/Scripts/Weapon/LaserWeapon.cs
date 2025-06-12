using System;
using System.Collections;
using Blocks;
using Car;
using Interfaces;
using Sound;
using UnityEngine;
using UnityEngine.Serialization;
using Upgrade;

namespace Weapon
{
    public class LaserWeapon : MonoBehaviour
    {
        [SerializeField] private float baseDamage;          //the base damage the laser does when hitting an enemy per frame * deltatime
        [SerializeField] private float fireTime;            //the time the laser will fire before turning off
        [SerializeField] private float fireDelay;           //the time the laser is turned off before firing
        [SerializeField] private float baseRange;           //the base range that the laser visual has
        [SerializeField] private LayerMask laserLayerMask;  //layer mask for what layers the laser can hit
        [SerializeField] private GameObject laser;          //the ref to the laser visual object
        [SerializeField] private Transform rayCastOrigin;   //the ref to the transform of where the raycast for finding targets originates from
        [SerializeField] private SoundService audioSource;  //Sound service for playing sound clips
        
        private float _damage;                              //The damage that the projectile will do when it hits an enemy, updates on upgrades
        private bool _canFire;                              //fires the laser on true and disables on false

        private bool CanFire                                //getter/setter for _canFire, when changed will reset the FireTimer and when turned false will disable the laser visual
        {
            get => _canFire;
            set
            {
                _canFire = value;
                if (value == false) ScaleLaser(Vector3.zero, true); 
                StartCoroutine(FireTimer());
            }
        }

        /// <summary>
        /// starts the FireTimer loop
        /// </summary>
        private void Start()
        {
            AssignEvents();
            IncreaseDamage();
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
        /// assigns events
        /// </summary>
        private void AssignEvents()
        {
            if (!UpgradeManager.Instance.OnUpgradeCompleted.TryAdd(BlockType.Weapon, IncreaseDamage)) UpgradeManager.Instance.OnUpgradeCompleted[BlockType.Weapon] += IncreaseDamage;
        }

        /// <summary>
        /// casts a raycast forward and checks to see if it hit an enemy, will dela damage when an enemy is found, then scales the laser visual to the hitpoint
        /// </summary>
        private void Shoot()
        {
            var laserBeam = laser;
            var rayDirection = laserBeam.transform.right;
            
            audioSource.PlaySound();
            
            if (!Physics.Raycast(rayCastOrigin.position, rayDirection, out var hitInfo, Mathf.Infinity,  laserLayerMask, QueryTriggerInteraction.Ignore))
            {
                ScaleLaser(Vector3.zero);
                return;
            }
            ScaleLaser(hitInfo.point);
            if (!hitInfo.collider.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(_damage * Time.deltaTime);
        }

        /// <summary>
        /// Scales the visual laser to "hitPoint" location, will scale it to base range when hitPoints is 0,0,0
        /// Will set the scale to zero if isEnabled is true
        /// </summary>
        /// <param name="hitPoint"></param>
        private void ScaleLaser(Vector3 hitPoint, bool isDisabled = false)
        {
            var laserBeam = laser;
            var scale = laserBeam.transform.localScale;
            
            if (isDisabled)
            {
                scale.x = 0;
                laserBeam.transform.localScale = scale;
                return;
            }
            
            var distance = hitPoint != Vector3.zero ? Vector3.Distance(hitPoint, laserBeam.transform.position) : baseRange;

            scale.x = distance; 
            laserBeam.transform.localScale = scale;
        }
        
        /// <summary>
        /// takes the bseDamage of the weapon and multiplies it by the WeaponAttackMultiplier
        /// </summary>
        private void IncreaseDamage()
        {
            _damage = CarData.Instance.WeaponAttackMultiplier * baseDamage;
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
