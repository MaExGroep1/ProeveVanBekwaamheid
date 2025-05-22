using System;
using System.Collections;
using Enemy;
using Interfaces;
using Unity.VisualScripting;
using UnityEngine;

namespace Weapon
{
    public class WeaponProjectile : MonoBehaviour
    {
        [SerializeField] private float destroyDelay;                    //The time before the ammo will be destroyed after it's shoot cycle it's completed
        [SerializeField] private LeanTweenType movementEasingType;      //The leanTween easing used for the movement of the projectile
        [SerializeField] private float rotationTime;                    //The time it takes for the projectile to rotate towards its target upo being shot
        [SerializeField] private LeanTweenType rotationEasingType;      //The leanTween rotation used for rotating towards the target
        [SerializeField] private Collider projectileCollider;           //The collider for the projectile that should bee turned on the moment it starts shooting
        
        private EnemyBehaviour _target;                                 //The target that the projectile should move to
        private float _damage;                                          //The damage that the projectile will do when it hits an enemy
        private float _travelTime;                                      //The damage that the projectile will do when it hits an enemy
        private float _range;                                           //The distance that the projectile will travel when there are no targets

        /// <summary>
        /// onCollision checks if collision target is IDamagable, deals damage to the target on hit
        /// and cancels any leantweens happening to stop the projectile from moving
        /// </summary>
        /// <param name="other">the hit collider</param>
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(_damage);
            LeanTween.cancel(gameObject, true);
        }

        /// <summary>
        /// Initializes the ammunition when spawned
        /// </summary>
        /// <param name="damage">The damage that the projectile will do when it hits an enemy</param>
        /// <param name="speed">The damage that the projectile will do when it hits an enemy</param>
        /// <param name="range">The distance that the projectile will travel when there are no targets</param>
        public void Initialize(float damage, float speed, float range)
        {
            _damage = damage;
            _travelTime = speed;
            _range = range;
        }
        
        /// <summary>
        /// moves the ammunition towards the target position
        /// if there is no target will move forward by the amount of _range
        /// also rotates the ammunition towards the target
        /// when the movement is complete, will call delayed destroy to avoid unused objets in the scene
        /// </summary>
        /// <param name="target">the target where to move towards</param>
        public void Shoot(EnemyBehaviour target)
        {
            var hitTarget = target != null ? target.transform.position : Vector3.right * _range + transform.position;
            var direction = (hitTarget - transform.position).normalized;
            var lookRotation = Quaternion.LookRotation(direction);
            var eulerRotation = lookRotation.eulerAngles;
            transform.parent = null;
            projectileCollider.enabled = true;
            LeanTween.move(gameObject, hitTarget, _travelTime).setEase(movementEasingType).setOnComplete(() =>
            {
                transform.AddComponent<Rigidbody>();
                Destroy(gameObject, destroyDelay);
            });
            LeanTween.rotate(gameObject, eulerRotation, rotationTime).setEase(rotationEasingType);
        }
    }
}
