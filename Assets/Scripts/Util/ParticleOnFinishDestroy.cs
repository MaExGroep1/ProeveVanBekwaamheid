using System;
using UnityEngine;

namespace Util
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleOnFinishDestroy : MonoBehaviour
    {
        /// <summary>
        /// Destroys the particle after it is done
        /// </summary>
        [Obsolete("Obsolete")]
        private void Awake()
        {
            var parts = GetComponent<ParticleSystem>();
            var totalDuration = parts.duration + parts.startLifetime;
            Destroy(gameObject, totalDuration);
        }
    }
}
