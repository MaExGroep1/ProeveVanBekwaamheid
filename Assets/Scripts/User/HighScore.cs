using UnityEngine;

namespace User
{
    public abstract class HighScore
    {
        public static string[] Keys => new[]    // the PlayerPref keys
        {
            "firstHighestScore",
            "secondHighestScore",
            "thirdHighestScore",
            "fourthHighestScore"
        };
        
        /// <summary>
        /// Tries to add the new score to the top 4
        /// </summary>
        /// <param name="newScore"> The new score to add </param>
        public static void SetScore(int newScore)
        {
            var scores = new int[4];
            for (var i = 0; i < 4; i++)
                scores[i] = PlayerPrefs.HasKey(Keys[i]) ? PlayerPrefs.GetInt(Keys[i]) : 0;
            
            var allScores = new[] { scores[0], scores[1], scores[2], scores[3], newScore };
            System.Array.Sort(allScores);
            System.Array.Reverse(allScores);

            for (var i = 0; i < 4; i++)
                PlayerPrefs.SetInt(Keys[i], allScores[i]);
            
            PlayerPrefs.Save();
        }
    }
}