using Enemy;
using UnityEngine;

namespace Weapon
{
    public class WeaponProjectile : MonoBehaviour
    {
        [SerializeField] private LeanTweenType leanTweenType;
        
        private EnemyBehaviour _target;
        private float _damage;
        private float _speed;
        
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

        public void Shoot(EnemyBehaviour target, float damage, float speed)
        {
            LeanTween.move(gameObject, target.transform.position, speed).setEase(leanTweenType).setOnComplete(()=> { Destroy(gameObject); });
        }
        
    }
}
