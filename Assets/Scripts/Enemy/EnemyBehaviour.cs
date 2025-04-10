using System;
using UnityEngine;

namespace Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        [SerializeField] private int health;
        [SerializeField] private int strength;
        [SerializeField] private int speed;
        [SerializeField] private int defense;

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            if (health>0) return;
            DestroySelf();
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }
    }
}
