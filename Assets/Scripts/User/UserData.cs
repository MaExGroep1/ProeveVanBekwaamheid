using System;
using Util;

namespace User
{
    public class UserData : Singleton<UserData>
    {
        private int _score;                                             //The total score of the current run
        private float _distanceTraveled;                                //The distance traveled in the current run
        private int _enemiesKilled;                                     //The amount of enemies killed in the current run
        private int _enemiesKilledScore;                                //The amount of points gained by killing enemies in the current run
        public int Score => _score;                                     //getter for _score
        public int EnemiesKilledScore  => _enemiesKilledScore;          //getter for _enemiesKilledScore
        public int EnemiesKilled => _enemiesKilled;                     //getter for _enemiesKilled
        public float DistanceTraveled => _distanceTraveled;             //getter for _distanceTraveled
        
        public Action<int> OnScoreChange;      //Invokes when score increases
        public Action<int> OnEnemyKilled;      //Invoked whenever an enemy dies, increases the score by <int>
        
        /// <summary>
        /// Gets called whenever an enemy dies and updates the scores and enemies killed count
        /// </summary>
        /// <param name="scoreChange"></param>
        public void CountEnemyKill(int scoreChange)
        {
            _enemiesKilled++;
            _enemiesKilledScore += scoreChange; 
            _score += scoreChange;
        }
        
        /// <summary>
        /// Changes the score, by scoreChange amount
        /// </summary>
        /// <param name="scoreChange"></param>
        public void ChangeScore(int scoreChange)
        {
            _score += scoreChange;
        }

    }
}
