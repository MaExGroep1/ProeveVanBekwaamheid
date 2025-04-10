using System;
using System.Collections.Generic;
using CarGame;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField] private FlyingEnemy flyingEnemyPrefab;
        [SerializeField] private GroundEnemy groundEnemyPrefab;
        
        private List<EnemyData> _groundEnemies;
        private List<EnemyData> _flyingEnemies;
        
        private List<EnemyBehaviour> _enemies = new(); 

        private static LevelData CurrentLevel => CarGameManager.Instance.CurrentLevel;      // the level data of the current level
        private EnemyData RandomGroundEnemies => _groundEnemies[Random.Range(0, _groundEnemies.Count)];
        private EnemyData RandomFlyingEnemies => _flyingEnemies[Random.Range(0, _flyingEnemies.Count)];

        private void Start() => CarGameManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        

        private void OnEnterNextLevel()
        {
            foreach (var groundEnemy in CurrentLevel.groundEnemies)
            {
                if (_groundEnemies.Contains(groundEnemy)) continue;
                _groundEnemies.Add(groundEnemy);
            }
            
            foreach (var flyingEnemy in CurrentLevel.flyingEnemies)
            {
                if (_flyingEnemies.Contains(flyingEnemy)) continue;
                _flyingEnemies.Add(flyingEnemy);
            }
        }

        public void CreateEnemy(EnemySpawner spawner)
        {
            var newEnemy = spawner.InAir ? 
                spawner.CreateEnemy(RandomFlyingEnemies, flyingEnemyPrefab):
                spawner.CreateEnemy(RandomGroundEnemies, groundEnemyPrefab);
            _enemies.Add(newEnemy);
        }
    }
}
