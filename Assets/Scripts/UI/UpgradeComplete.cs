using System;
using System.Collections;
using ntw.CurvedTextMeshPro;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UpgradeComplete : MonoBehaviour
    {
        [SerializeField] private Transform targetLocation;
        [SerializeField] private RectTransform text;
        [SerializeField] private CanvasGroup textGroup;
        [SerializeField] private float duration;
        [SerializeField] private float fadeStart;
        [SerializeField] private UpgradeUiBehaviour upgradeUiBehaviour;

        private void Awake() => upgradeUiBehaviour.AddListener(StartPopUp);

        private void StartPopUp() => StartCoroutine(PopUp());

        private IEnumerator PopUp()
        {
            LeanTween.moveY(text.gameObject, targetLocation.position.y, duration);
            
            yield return new WaitForSeconds(fadeStart);

            LeanTween.alphaCanvas(textGroup, 0, duration - fadeStart);
            
            yield return new WaitForSeconds(duration - fadeStart);

            text.localPosition = Vector3.zero;
            
            textGroup.alpha = 1;
        }
    }
}
