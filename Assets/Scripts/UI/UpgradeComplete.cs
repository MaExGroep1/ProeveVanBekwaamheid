using System;
using System.Collections;
using ntw.CurvedTextMeshPro;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpgradeComplete : MonoBehaviour
    {
        [SerializeField] private Transform targetLocation;              // the target location when going up
        [SerializeField] private TextMeshProUGUI text;                    // the element to move up
        [SerializeField] private CanvasGroup textGroup;                 // the group to fade off
        [SerializeField] private float duration;                        // the lenght the animation takes
        [SerializeField] private float fadeStart;                       // the lenght before the alpha starts to fade
        [SerializeField] private UpgradeUiBehaviour upgradeUiBehaviour; // the upgrade to listen to

        private int _level;

        /// <summary>
        /// Adds listeners to the upgrade element
        /// </summary>
        private void Awake() => upgradeUiBehaviour.AddListener(StartPopUp);

        private void StartPopUp() => StartCoroutine(PopUp());

        /// <summary>
        /// Moves the Text up to the target then resets it
        /// </summary>
        /// <returns></returns>
        private IEnumerator PopUp()
        {
            _level++;
            text.text = $"Lvl {_level}";
            LeanTween.moveY(text.gameObject, targetLocation.position.y, duration);
            
            yield return new WaitForSeconds(fadeStart);

            LeanTween.alphaCanvas(textGroup, 0, duration - fadeStart);
            
            yield return new WaitForSeconds(duration - fadeStart);

            text.transform.localPosition = Vector3.zero;
            
            textGroup.alpha = 1;
        }
    }
}
