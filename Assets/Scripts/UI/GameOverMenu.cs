using System;
using System.Collections;
using Enemy;
using TMPro;
using UnityEngine;
using User;

namespace UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [Header("New High Score Text")]
        [SerializeField] private GameObject newHighScore;           // the "NEW HIGH SCORE" text element
        [SerializeField] private float newHighScoreAppearTime;      // the time it takes for the "NEW HIGH SCORE" text to appear
        [SerializeField] private float newHighScoreWaitTime;        // the wait before the "NEW HIGH SCORE" text appears
        [Header("Text Elements")]
        [SerializeField] private TextMeshProUGUI scoreText;         // the new score text
        [SerializeField] private TextMeshProUGUI highScoreText;     // the high score text
        [SerializeField] private TextMeshProUGUI enemyScoreText;     // the high score text
        [Header("Enemy Points")] 
        [SerializeField] private float enemyPointsAddTime;

        private void Awake() => StartCoroutine(SetDisplay());
        
        /// <summary>
        /// Summons the new high score text
        /// </summary>
        private void WaitForAnimation()
        {
            newHighScore.SetActive(true);
            
            newHighScore.transform.localScale = Vector3.zero;

            LeanTween.scale(newHighScore, Vector3.one, newHighScoreAppearTime)
                .setEase(LeanTweenType.easeOutBack);
        }

        /// <summary>
        /// Sets the score display and checks if you have a new high score
        /// Then submits the score to the high score class
        /// </summary>
        private IEnumerator SetDisplay()
        {
            var highScore = PlayerPrefs.GetInt(HighScore.Keys[0],0);
            var newScore = UserData.Instance.DistanceScore;
            var enemyScore = EnemyManager.Instance.EnemyPoints;
            var total = Mathf.RoundToInt(newScore + enemyScore);

            
            highScoreText.text = $"High Score: {highScore.ToString("N0").Replace(',', '.')}";
            scoreText.text = $"Score: {Mathf.RoundToInt(newScore).ToString("N0").Replace(',', '.')}";
            enemyScoreText.text = $"+{Mathf.RoundToInt(enemyScore).ToString("N0").Replace(',', '.')}";

            yield return AddEnemyPoints(newScore,EnemyManager.Instance.EnemyPoints);

            if (highScore < newScore)
                WaitForAnimation();
            
            HighScore.SetScore(total);
        }

        private IEnumerator AddEnemyPoints(float score, float enemy)
        {
            yield return new WaitForSeconds(newHighScoreWaitTime);

            var timer = 0f;
            var total = score + enemy;
            while (timer < enemyPointsAddTime)
            {
                timer += Time.deltaTime;

                var current = Mathf.RoundToInt(Mathf.Lerp(score, total, timer/enemyPointsAddTime));
                var enemyDisplay = Mathf.RoundToInt(Mathf.Lerp(enemy, 0, timer/enemyPointsAddTime));
                
                scoreText.text = $"Score: {current.ToString("N0").Replace(',', '.')}";
                enemyScoreText.text = $"+{enemyDisplay.ToString("N0").Replace(',', '.')}";
                yield return null;
            }
            var displayTotal = Mathf.RoundToInt(total);

            enemyScoreText.gameObject.SetActive(false);
            scoreText.text = $"Score: {displayTotal.ToString("N0").Replace(',', '.')}";
        }
    }
}
