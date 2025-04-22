using System;
using System.Collections;
using Testing;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private float health;              // the health of the enemy
        [SerializeField] private int strength;              // the strength of the enemy
        [SerializeField] private int speed;                 // the speed of the enemy
        [SerializeField] private int defense;               // the defense of the enemy
        
        [Header("Follow Rules")]
        [SerializeField] private bool followsOnX;           // whether the enemy should follow the target on the x-axis 
        [SerializeField] private bool followsOnY;           // whether the enemy should follow the target on the y-axis 
        
        [Header("Near Player")]
        [SerializeField] private float nearPlayerDistance;  // the distance the enemy will stop chasing the player
        
        private Rigidbody _rigidBody;                       // the enemy's rigidbody
        private Transform _target;                          // the transform of the target
        private bool _hasBeenNearPlayer;                    // if the player has been near the player
        private Action _nearPlayer;                         // invokes ones the enemy gets near the player

        private void Awake() => _rigidBody = GetComponent<Rigidbody>();
        
        /// <summary>
        ///  Moves the enemy closer to the target
        /// </summary>
        private void Update()
        {
            var x = followsOnX ? _target.position.x : transform.position.x;
            var y = followsOnY ? _target.position.y : transform.position.y;
            var targetPosition = new Vector3(x, y, 0);
            if (_rigidBody == null) return;
            _rigidBody.AddForce((targetPosition - transform.position).normalized * speed * _rigidBody.mass);
            if (_hasBeenNearPlayer || Vector3.Distance(transform.position, targetPosition) > nearPlayerDistance || !_target.gameObject.CompareTag("Player")) return;
            _nearPlayer?.Invoke();
            _hasBeenNearPlayer = true;
        }
        
        /// <summary>
        /// Checks if the player is close enough
        /// </summary>
        /// <param name="other"> The other collider </param>
        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            _target = other.transform;
        }
        
        /// <summary>
        /// Checks if it hit the player then calculates the impact
        /// </summary>
        /// <param name="other"></param>
        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var impact = Mathf.Abs(other.relativeVelocity.x) + Mathf.Abs(_rigidBody.velocity.x);
            other.gameObject.GetComponent<Player>().OnHitEnemy(this, _rigidBody, impact, defense, strength);
        }
        
        /// <summary>
        /// Sets the target to the spawner
        /// </summary>
        /// <param name="roamTarget"> the enemy spawner</param>
        public void StartRoam(Transform roamTarget) => _target = roamTarget;
        
        /// <summary>
        /// Destroys the enemy
        /// </summary>
        /// <param name="time"> the time to wait before destroying itself </param>
        public void DestroySelf(float time)
        {
            _target = transform;
            Destroy(GetComponent<Collider>());
            Destroy(_rigidBody);
            Destroy(gameObject,time);
        }
        
        /// <summary>
        /// Removes a certain amount of points off the enemy's health
        /// </summary>
        /// <param name="damage"> the amount of damage to take</param>
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health>0) return;
            EnemyManager.Instance.DestroyEnemy(this);
        }
    }
}
