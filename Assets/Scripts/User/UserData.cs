using UnityEngine;
using Util;

namespace User
{
    public class UserData : Singleton<UserData>
    {   
        private int _score;
        private float _distance;
        private int _enemiesKilled;
        private int _enemiesKilledScore;
        
        public int Score { get => _score; private set => _score = value; }
        public float Distance { get => _distance; set => _distance = value; }
        public int EnemiesKilled { get => _enemiesKilled; set => _enemiesKilled = value; }
        public int EnemiesKilledScore { get => _enemiesKilledScore; private set => _enemiesKilledScore = value; }


    }
}
