using TMPro;
using UnityEngine;
using UnityEngine.UI;
using User;

namespace UI
{
    public class HighScoreDisplay : MonoBehaviour
    {
        [Header("Text Fields")]
        [SerializeField] private TextMeshProUGUI[] highScoreTexts = new TextMeshProUGUI[4]; // the high score text elements
        [SerializeField] private string noScoreText = "-";                                  // the text if there is no score

        [Header("Box")]
        [SerializeField] private GameObject boxTransform;                                   // the high score menu element
        [SerializeField] private Transform boxStartTransform;                               // the starting position off-screen
        [SerializeField] private CanvasGroup backgroundCanvasGroup;                         // the blur in the background
        [SerializeField] private float targetAlpha = 1f;                                    // the target alpha of the background

        [Header("Box Animations")] 
        [SerializeField] private float boxTravelTime = 0.5f;                                // the time the box takes to animate to the correct position

        [Header("Escape Button")]
        [SerializeField] private Button escapeButton;                                       // the button to close the menu

        private void Awake() => Starts();
        
        /// <summary>
        /// Starts to listen to all the buttons then moves the box up
        /// </summary>
        private void Starts()
        {
            escapeButton.onClick.AddListener(AnimateBoxOut);
            SetText();
            AnimateBoxIn();
        }

        
        /// <summary>
        /// Sets the high score texts to the corresponding number
        /// </summary>
        private void SetText()
        {
            for (var i = 0; i < highScoreTexts.Length && i < HighScore.Keys.Length; i++)
            {
                var key = HighScore.Keys[i];
                var hasScore = PlayerPrefs.HasKey(key) && PlayerPrefs.GetInt(key) > 0;
                highScoreTexts[i].text = hasScore ? PlayerPrefs.GetInt(key).ToString() : noScoreText;
            }
        }
        
        /// <summary>
        /// Moves the high score box in frame and blurs the backdrop
        /// </summary>
        private void AnimateBoxIn()
        {
            boxTransform.transform.position = boxStartTransform.position;
            backgroundCanvasGroup.alpha = 0f;

            LeanTween.moveLocalY(boxTransform, 0, boxTravelTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeOutBack);
            LeanTween.alphaCanvas(backgroundCanvasGroup, targetAlpha, boxTravelTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeOutSine);
        }

        /// <summary>
        /// Moves the high score box out of frame and un-blurs the backdrop
        /// </summary>
        private void AnimateBoxOut()
        {
            LeanTween.moveY(boxTransform, boxStartTransform.position.y, boxTravelTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInBack);
            LeanTween.alphaCanvas(backgroundCanvasGroup, 0, boxTravelTime)
                .setIgnoreTimeScale(true)
                .setEase(LeanTweenType.easeInSine)
                .setOnComplete(() => Destroy(gameObject));
        }
    }
}