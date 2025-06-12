using System;
using System.Collections;
using Sound;
using UnityEngine;

namespace Car
{
    public class CarShockWave : MonoBehaviour
    {
        [Header("Shockwave Settings")]
        [SerializeField] private float radius;                  // the radius of the blast
        [SerializeField] private float thrust;                  // the amount of force of the blast
        [SerializeField] private float upwardsThrustMultiplier; // the extra amount of force upwards
        
        [Header("Sound")]
        [SerializeField] private SoundService audioSource;      // the blast audio
        
        [Header("Ripple")]
        [SerializeField] private Material rippleMaterial;       // the material that has the ripple shader
        [SerializeField] private float rippleDuration;          // the duration of the ripple effect
        [SerializeField] private float rippleOffset;            // the offset of the time of the ripple
        [SerializeField] private RectTransform carScreenspace;  // the offset of the time of the ripple
        
        [SerializeField] private string rippleTimeProperty;     // the name of the time property in the "rippleMaterial" shader
        [SerializeField] private string ripplePositionProperty; // the name of the position property in the "rippleMaterial" shader
        [SerializeField] private string rippleAspectProperty;   // the name of the aspect property in the "rippleMaterial" shader

        private Vector2 _screenSize;
        private Coroutine _rippleCoroutine;                     // coroutine of the active ripple

        /// <summary>
        /// Sets the screenSize and aspect in the ripple shader
        /// </summary>
        private void Awake()
        {
            _screenSize = new Vector2(Screen.width, Screen.height);
            rippleMaterial.SetVector(rippleAspectProperty , _screenSize);
        }

        /// <summary>
        /// Blasts all nearby enemies away from the car and creates a ripple effect
        /// </summary>
        public void Shockwave()
        {
            var colliders = Physics.OverlapSphere(transform.position, radius);
            var position = new Vector2(carScreenspace.position.x, carScreenspace.position.y) / _screenSize;

            foreach (var hit in colliders)
            {
                if (!hit.gameObject.CompareTag("Enemy")) continue;
                hit.TryGetComponent<Rigidbody>(out var rb);
                if (rb != null )rb.AddExplosionForce(thrust, transform.position, radius, upwardsThrustMultiplier);
            }
            
            rippleMaterial.SetVector(ripplePositionProperty , position);
            if (_rippleCoroutine != null) StopCoroutine(_rippleCoroutine);
            _rippleCoroutine = StartCoroutine(Ripple());
            
            audioSource.PlaySound();
        }

        /// <summary>
        /// Creates a ripple on screen
        /// </summary>
        /// <returns></returns>
        private IEnumerator Ripple()
        {
            var time = 0f;
            rippleMaterial.SetFloat(rippleTimeProperty , rippleOffset);
            while (time < rippleDuration)
            {
                time += Time.deltaTime;
                rippleMaterial.SetFloat(rippleTimeProperty , time / rippleDuration + rippleOffset);
                yield return null;
            }
            rippleMaterial.SetFloat(rippleTimeProperty , rippleOffset);
        }
    }
}
