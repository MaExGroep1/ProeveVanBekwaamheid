using System;
using Car;
using Interfaces;
using Sound;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour, IDamageable
    {

        [Header("Stats")]
        [SerializeField] private float health;              // the health of the enemy
        [SerializeField] private float attack;              // the attack of the enemy
        [SerializeField] private float speed;               // the speed of the enemy
        [SerializeField] private float defense;             // the defense of the enemy
        
        [Header("Follow Rules")]
        [SerializeField] private bool followsOnX;           // whether the enemy should follow the target on the x-axis 
        [SerializeField] private bool followsOnY;           // whether the enemy should follow the target on the y-axis 
        
        [Header("Near Player")]
        [SerializeField] private float nearPlayerDistance;  // the distance the enemy will stop chasing the player
        
        [Header("audio")]
        [SerializeField] private SoundService soundService;  // sound service for playing sound clips
        
        private Rigidbody _rigidBody;                       // the enemy's rigidbody
        private Transform _target;                          // the transform of the target
        private bool _hasBeenNearPlayer;                    // if the player has been near the player

        private Action _nearPlayer;                         // invokes ones the enemy gets near the player
        
        public float Worth { get; private set; }            // the amount of points the user gets when killing the enemy
        public bool MarkedForDeletion { get; private set; } // whether the enemy has been marked for deletion

        
        private void Awake() =>_rigidBody = GetComponent<Rigidbody>();
        

        private void Update()
        {
            if (_rigidBody == null) return;
            MoveToTarget();
        }

        /// <summary>
        ///  Moves the enemy closer to the target
        /// </summary>
        private void MoveToTarget()
        {
            var x = followsOnX ? _target.position.x : transform.position.x;
            var y = followsOnY ? _target.position.y : transform.position.y;
            
            var targetPosition = new Vector3(x, y, 0);
            
            _rigidBody.AddForce((targetPosition - transform.position).normalized * (speed * _rigidBody.mass));
            
            if (_hasBeenNearPlayer || Vector3.Distance(transform.position, targetPosition) > nearPlayerDistance || !_target.gameObject.CompareTag("Player")) 
                return;
            
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
        /// Checks if it hit the player or another enemy
        /// </summary>
        /// <param name="other"> the collision </param>
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                EnterRagdoll();
                PlayerHit(other);
            }
            else if (other.gameObject.CompareTag("Enemy"))
                OtherEnemyHit(other);
        }

        /// <summary>
        /// Checks if it is on the player
        /// </summary>
        /// <param name="other"> the collision </param>
        private void OnCollisionStay(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            PlayerHit(other);
        }
        
        /// <summary>
        /// Gets the player's script then applies the force
        /// </summary>
        /// <param name="player"> the collision with the player </param>
        private void PlayerHit(Collision player) =>
            player.gameObject.GetComponent<CarEnemyInteraction>().OnHitEnemy
            (
                this,
                _rigidBody,
                Mathf.Abs(player.relativeVelocity.x), 
                Mathf.Abs(_rigidBody.velocity.x), 
                defense, 
                attack
            );
        
        /// <summary>
        /// Gets the enemy's script then applies force to it
        /// </summary>
        /// <param name="otherEnemy"> the collision with the other enemy </param>
        private void OtherEnemyHit(Collision otherEnemy) =>
            otherEnemy.gameObject.GetComponent<EnemyBehaviour>().OnHitByEnemy
                (Mathf.Abs(_rigidBody.velocity.x * 2));
        
        /// <summary>
        /// Applies force forward
        /// </summary>
        /// <param name="force"> the hit enemies force on the x-axis </param>
        private void OnHitByEnemy(float force) =>
            _rigidBody.AddForce(new Vector3(0,force));

        /// <summary>
        /// Unfreezes the rotations
        /// </summary>
        private void EnterRagdoll() => 
            _rigidBody.constraints = RigidbodyConstraints.None;
        

        /// <summary>
        /// Applies the multiplier to the attack defence and health then sets the worth
        /// </summary>
        /// <param name="multiplier"> the point multiplier </param>
        public void ApplyMultiplier(float multiplier)
        {
            health *= multiplier;
            attack *= multiplier;
            defense *= multiplier;
            Worth = attack + defense;
            if (multiplier<1)
                Destroy(gameObject);
        }
        
        /// <summary>
        /// Sets the target to the spawner
        /// </summary>
        /// <param name="roamTarget"> the enemy spawner</param>
        public void StartRoam(Transform roamTarget) => _target = roamTarget;
        
        /// <summary>
        /// Destroys the rigidbody
        /// </summary>
        public void DestroyRigidbody() => Destroy(_rigidBody);
                
        /// <summary>
        /// Removes a certain amount of points off the enemy's health
        /// </summary>
        /// <param name="damage"> the amount of damage to take</param>
        public void TakeDamage(float damage)
        {
            health -= damage;
            if (health > 0) return;

            soundService.PlaySound();
            MarkedForDeletion = true;
            gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
            EnemyManager.Instance.DestroyEnemy(this);
        }
    }
}
