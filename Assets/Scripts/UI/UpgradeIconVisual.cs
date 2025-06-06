using System.Collections;
using UnityEngine;

namespace UI
{
    public class UpgradeIconVisual : MonoBehaviour
    {
        [SerializeField] private Transform targetLocation;                  // the target location when going up
        [SerializeField] private UpgradeUiBehaviour upgradeUiBehaviour;     // the upgrade to listen to
        [Header("Animation")]
        [SerializeField] private float animationTime;                       // the lenght the animation takes
        [SerializeField] private float fadeStartPercent;                    // the lenght before the alpha starts to fade
        [SerializeField] private float targetScale;                         // the lenght before the alpha starts to fade
        [Header("Icon")]
        [SerializeField] private GameObject icon;                           // the element to move up
        [SerializeField] private CanvasGroup canvasGroup;                   // the group to fade off

        private Coroutine _animation;
        
        /// <summary>
        /// Adds listeners to the upgrade element
        /// </summary>
        private void Awake() =>
            upgradeUiBehaviour.AddListener(StartAnimation );

        private void StartAnimation()
        {
            if(_animation != null) StopCoroutine(_animation);
            _animation = StartCoroutine(Animation());
        }
        
        /// <summary>
        /// Moves the Text up to the target then resets it
        /// </summary>
        /// <returns></returns>
        private IEnumerator Animation()
        {
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            
            canvasGroup.alpha = 1;

            var distance = Vector3.Distance(transform.position, targetLocation.position);
            var time = animationTime * distance;
            var wait = time * fadeStartPercent;

            LeanTween.cancel(icon);
            
            LeanTween.moveX(icon, targetLocation.position.x, time).setEase(LeanTweenType.easeInSine);
            LeanTween.moveY(icon, targetLocation.position.y, time).setEase(LeanTweenType.easeOutSine);
            
            yield return new WaitForSeconds(wait);

            LeanTween.alphaCanvas(canvasGroup, 0, time - wait);
            LeanTween.scale(icon, Vector3.one * targetScale, time - wait);
        }

    }
}
