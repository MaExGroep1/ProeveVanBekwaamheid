using System;
using Unity.VisualScripting;
using UnityEngine;

namespace User
{
    public class UserData : Util.Singleton<UserData>
    {   
        private int _score;
        private float _distanceTraveled;
        private int _enemiesKilled;
        private int _enemiesKilledScore;
        
        public int Score { get => _score; private set => _score = value; }
        public int EnemiesKilledScore { get => _enemiesKilledScore; private set => _enemiesKilledScore = value; }
        public int EnemiesKilled { get => _enemiesKilled; set => _enemiesKilled = value; }
        
        public float DistanceTraveled { get => _distanceTraveled; set => _distanceTraveled = value; }
        
        public Action<int> OnScoreChange;
        public Action<int> OnEnemyKilled;


        private void Awake()
        {
            AssignEvents();
        }

        private void OnDestroy()
        {
            UnAssignEvents();
        }

        private void EnemyKilled(int scoreChange)
        {
            EnemiesKilled++;
            EnemiesKilledScore += scoreChange; 
            Score += scoreChange;
        }
        
        private void ScoreChange(int scoreChange)
        {
            Score += scoreChange;
        }
        
        private void AssignEvents()
        {
            OnScoreChange += ScoreChange;
            OnEnemyKilled += EnemyKilled;
        }
        
        private void UnAssignEvents()
        {
            OnScoreChange -= ScoreChange;
            OnEnemyKilled -= EnemyKilled;
        }
    }
}
