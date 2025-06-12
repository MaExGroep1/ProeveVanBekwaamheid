using System;
using System.Collections;
using ntw.CurvedTextMeshPro;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UpgradeComplete : MonoBehaviour
    {
        [SerializeField] private Transform targetLocation;              // the target location when going up
        [SerializeField] private UpgradeUiBehaviour upgradeUiBehaviour; // the upgrade to listen to
        [SerializeField] private Button popUpButton;                    // the button to listen to
        [Header("Animation")]
        [SerializeField] private float duration;                        // the lenght the animation takes
        [SerializeField] private float fadeStart;                       // the lenght before the alpha starts to fade
        [Header("Text")]
        [SerializeField] private TextMeshProUGUI text;                  // the element to move up
        [SerializeField] private CanvasGroup textGroup;                 // the group to fade off
        [SerializeField] private string textPreFix;                     // the prefix of the upgrade pop up


        private int _level;

        /// <summary>
        /// Adds listeners to the upgrade element
        /// </summary>
        private void Awake()
        {
            popUpButton.onClick.AddListener(PopUp);
            upgradeUiBehaviour.AddListener(PopUpPlusOne);
        }

        private void PopUp() => StartCoroutine(TextPopUp());

        /// <summary>
        /// Adds one then creates pop up
        /// </summary>
        private void PopUpPlusOne()
        {
            _level++;
            StartCoroutine(TextPopUp());
        }
        
        /// <summary>
        /// Moves the Text up to the target then resets it
        /// </summary>
        /// <returns></returns>
        private IEnumerator TextPopUp()
        {
            LeanTween.cancel(text.gameObject);
            
            text.transform.localPosition = Vector3.zero;
            
            textGroup.alpha = 1;
            
            text.text = $"{textPreFix}\\nLvl {_level}";
            
            LeanTween.moveY(text.gameObject, targetLocation.position.y, duration);
            
            yield return new WaitForSeconds(fadeStart);

            LeanTween.alphaCanvas(textGroup, 0, duration - fadeStart);
        }
    }
}
