using System;
using UnityEngine;

namespace Util
{
    public class IdleShake : MonoBehaviour
    {
        [SerializeField] private float shakeDuration;   // the total time to move from left to right
        [SerializeField] private float shakeTarget;     // the target angle of the idle
        
        /// <summary>
        /// Rotates the element to the negative target rotation
        /// then starts the loop
        /// </summary>
        private void Awake() =>
            LeanTween.rotateZ(gameObject, -shakeTarget, shakeDuration / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(StartLoop);
        
        /// <summary>
        /// Rotates the element to the target then loops it back and forth
        /// </summary>
        private void StartLoop() => 
            LeanTween.rotateZ(gameObject, shakeTarget, shakeDuration)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInOutQuad)
                .setLoopPingPong();
    }
}
