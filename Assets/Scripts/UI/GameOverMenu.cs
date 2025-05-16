using System;
using System.Collections;
using TMPro;
using UnityEngine;
using User;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] private GameObject newHighScore;           // the "NEW HIGH SCORE" text element
        [SerializeField] private float newHighScoreAppearTime;      // the time it takes for the "NEW HIGH SCORE" text to appear
        [SerializeField] private float newHighScoreWaitTime;        // the wait before the "NEW HIGH SCORE" text appears
        [SerializeField] private TextMeshProUGUI scoreText;         // the new score text
        [SerializeField] private TextMeshProUGUI highScoreText;     // the high score text

        private void Awake() => SetDisplay();
        /// <summary>
        /// Sets the score display and checks if you have a new high score
        /// Then submits the score to the high score class
        /// </summary>
        private void SetDisplay()
        {
            var highScore = PlayerPrefs.GetInt(HighScore.Keys[0],0);
            var newScore = UserData.Instance.Score;
            
            highScoreText.text = $"High Score: {highScore.ToString("N0").Replace(',', '.')}";
            scoreText.text = $"Score: {newScore.ToString("N0").Replace(',', '.')}";

            if (highScore < newScore)
                StartCoroutine(WaitForAnimation());
            
            HighScore.SetScore(newScore);
        }
        
        /// <summary>
        /// Waits to summon the new high score text
        /// </summary>
        /// <returns></returns>
        private IEnumerator WaitForAnimation()
        {
            yield return new WaitForSeconds(newHighScoreWaitTime);
            
            newHighScore.SetActive(true);
            
            newHighScore.transform.localScale = Vector3.zero;

            LeanTween.scale(newHighScore, Vector3.one, newHighScoreAppearTime)
                .setEase(LeanTweenType.easeOutBack);
        }
    }
}
