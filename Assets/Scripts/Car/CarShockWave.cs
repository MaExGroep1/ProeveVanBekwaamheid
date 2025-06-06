using Sound;
using UnityEngine;

namespace Car
{
    public class CarShockWave : MonoBehaviour
    {
        [SerializeField] private float radius;
        [SerializeField] private float thrust;
        [SerializeField] private float upwardsThrust;
        [SerializeField] private SoundService audioSource;

        public void Shockwave()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius);

            foreach (var hit in colliders)
            {
                if (!hit.gameObject.CompareTag("Enemy")) continue;
                hit.TryGetComponent<Rigidbody>(out var rb);
                if (rb != null )rb.AddExplosionForce(thrust, transform.position, radius, upwardsThrust);
            }
            
            audioSource.PlaySound();
        }
    }
}
