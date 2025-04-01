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
        public int Score { get => _score; private set => _score = value; }                                              //getter/setter for _score
        public int EnemiesKilledScore { get => _enemiesKilledScore; private set => _enemiesKilledScore = value; }       //getter/setter for _enemiesKilledScore
        public int EnemiesKilled { get => _enemiesKilled; set => _enemiesKilled = value; }                              //getter/setter for _enemiesKilled
        public float DistanceTraveled { get => _distanceTraveled; set => _distanceTraveled = value; }                   //getter/setter for _distanceTraveled
        
        public Action<int> OnScoreChange;      //Will increase the score by <int>
        public Action<int> OnEnemyKilled;      //Invoked whenever an enemy dies, increases the score by <int>
        
        /// <summary>
        /// assigns events
        /// </summary>
        private void Awake()
        {
            AssignEvents();
        }

        /// <summary>
        /// Unassigns events onDestroy to avoid null refs
        /// </summary>
        private void OnDestroy()
        {
            UnAssignEvents();
        }

        
        /// <summary>
        /// Gets called whenever an enemy dies and updates the scores and enemies killed count
        /// </summary>
        /// <param name="scoreChange"></param>
        private void CountEnemyKill(int scoreChange)
        {
            EnemiesKilled++;
            EnemiesKilledScore += scoreChange; 
            Score += scoreChange;
        }
        
        /// <summary>
        /// Changes the score, by scoreChange amount
        /// </summary>
        /// <param name="scoreChange"></param>
        private void ChangeScore(int scoreChange)
        {
            Score += scoreChange;
        }
        
        /// <summary>
        /// assigns events
        /// </summary>
        private void AssignEvents()
        {
            OnScoreChange += ChangeScore;
            OnEnemyKilled += CountEnemyKill;
        }
        
        /// <summary>
        /// unassigns events
        /// </summary>
        private void UnAssignEvents()
        {
            OnScoreChange -= ChangeScore;
            OnEnemyKilled -= CountEnemyKill;
        }
    }
}
