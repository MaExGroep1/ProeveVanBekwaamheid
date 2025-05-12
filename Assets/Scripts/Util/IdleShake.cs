using System;
using UnityEngine;

namespace Util
{
    public class IdleShake : MonoBehaviour
    {
        [SerializeField] private float shakeDuration;
        [SerializeField] private float shakeTarget;

        private void Awake() =>
            LeanTween.rotateZ(gameObject, -shakeTarget, shakeDuration / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(StartLoop);

        private void StartLoop() => 
            LeanTween.rotateZ(gameObject, shakeTarget, shakeDuration / 2)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInOutQuad)
                .setLoopPingPong();
    }
}
