using System;
using System.Collections;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private int health;
        [SerializeField] private int strength;
        [SerializeField] private int speed;
        [SerializeField] private int defense;
        
        [Header("Follow Rules")]
        [SerializeField] private bool followsOnX;
        [SerializeField] private bool followsOnY;
        
        [Header("Near Player")]
        [SerializeField] private float nearPlayerDistance;
        
        private Rigidbody _rigidBody;
        
        private Transform _target;
        
        private bool _hasBeenNearPlayer;

        private Action _nearPlayer;

        private void Awake() => _rigidBody = GetComponent<Rigidbody>();
        

        private void Update()
        {
            var x = followsOnX ? _target.position.x : transform.position.x;
            var y = followsOnY ? _target.position.y : transform.position.y;
            var targetPosition = new Vector3(x, y, 0);
            _rigidBody.AddForce((targetPosition - transform.position).normalized * speed * _rigidBody.mass);
            if (_hasBeenNearPlayer || Vector3.Distance(transform.position, targetPosition) > nearPlayerDistance || !_target.gameObject.CompareTag("Player")) return;
            _nearPlayer?.Invoke();
            _hasBeenNearPlayer = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _target = other.transform;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var velocity =  Mathf.Abs(other.relativeVelocity.x) + Mathf.Abs(other.relativeVelocity.y);
            
        }

        public void DestroySelf(float time)
        {
            _target = transform;
            Destroy(gameObject,time);
        }

        public void StartRoam(Transform roamTarget) => _target = roamTarget;
        
        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health>0) return;
            DestroySelf(0);
        }
    }
}
