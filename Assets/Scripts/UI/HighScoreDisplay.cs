using TMPro;
using UnityEngine;
using User;

namespace UI
{
    public class HighScoreDisplay : MonoBehaviour
    {
        [Header("Text Fields")]
        [SerializeField] private TextMeshProUGUI[] highScoreTexts = new TextMeshProUGUI[4]; // the high score text elements
        [SerializeField] private string noScoreText = "-";                                  // the text if there is no score
        
        private void Awake() => SetText();
        
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
    }
}