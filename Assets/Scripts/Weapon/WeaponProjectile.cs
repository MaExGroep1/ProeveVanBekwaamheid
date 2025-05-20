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
        [SerializeField] private float destroyDelay;
        [SerializeField] private LeanTweenType movementEasingType;
        [SerializeField] private float rotationTime;
        [SerializeField] private LeanTweenType rotationEasingType;
        
        private EnemyBehaviour _target;
        private float _damage;
        private float _speed;
        private float _range;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(_damage);
            LeanTween.cancel(gameObject, true);
        }

        public void Initialize(float damage, float speed, float range)
        {
            _damage = damage;
            _speed = speed;
            _range = range;
        }
        public void Shoot(EnemyBehaviour target)
        {
            Vector3 hitTarget = target != null ? target.transform.position : Vector3.right * _range + transform.position;
            Vector3 direction = (hitTarget - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            Vector3 eulerRotation = lookRotation.eulerAngles;
            transform.parent = null;
            LeanTween.move(gameObject, hitTarget, _speed).setEase(movementEasingType).setOnComplete(() =>
            {
                transform.AddComponent<Rigidbody>();
                StartCoroutine(DelayedDestroy());
            });
            LeanTween.rotate(gameObject, eulerRotation, rotationTime).setEase(rotationEasingType);
        }

        private IEnumerator DelayedDestroy()
        {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(gameObject);
        }
    }
}
