using System;
using Enemy;
using Interfaces;
using UnityEngine;

namespace Weapon
{
    public class WeaponProjectile : MonoBehaviour
    {
        [SerializeField] private LeanTweenType leanTweenType;
        
        private EnemyBehaviour _target;
        private float _damage;
        private float _speed;
        
        public Action OnDestroyed;
        public EnemyBehaviour Target
        {
            get => _target;
            set => _target = value;
        }

        public float Damage
        {
            get => _damage;
            set => _damage = value;
        }

        public float Speed
        {
            get => _speed;
            set => _speed = value;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.TryGetComponent(out IDamageable target)) return;
            target.TakeDamage(_damage);
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            LeanTween.cancel(gameObject);
            OnDestroyed?.Invoke();
        }

        public void Initialize(float damage, float speed)
        {
            _damage = damage;
            _speed = speed;
        }
        public void Shoot(EnemyBehaviour target)
        {
            LeanTween.move(gameObject, target.transform.position, _speed).setEase(leanTweenType).setOnComplete(()=> { Destroy(gameObject); });
        }
        
        
        
    }
}
