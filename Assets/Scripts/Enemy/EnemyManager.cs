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
        [SerializeField] private ParticleSystem onDeathParticles;                                               // the enemy death particle
        [field: SerializeField] public float EnemyMultiplierAmount { get; private set; }                        // the amount of tiles to have instantiated before making the difficulty plus 1
        
        private readonly List<EnemyBehaviour> _groundEnemies = new();                                           // list of available ground enemies
        private readonly List<EnemyBehaviour> _flyingEnemies = new();                                           // list of available flying enemies
        private readonly List<EnemyBehaviour> _currentEnemies = new();                                          // list of current enemies
        private List<EnemyBehaviour> _enemies = new();                                                          // list of all enemies

        private float _enemyPoints;

        private static LevelData CurrentLevel => CarGameManager.Instance.CurrentLevel;                          // the level data of the current level
        private EnemyBehaviour RandomGroundEnemies => _groundEnemies[Random.Range(0, _groundEnemies.Count)];    // a random ground enemy
        private EnemyBehaviour RandomFlyingEnemies => _flyingEnemies[Random.Range(0, _flyingEnemies.Count)];    // a random flying enemy

        public List<EnemyBehaviour> Enemies
        {
            get => _enemies;
            private set => _enemies = value;
        }
        
        /// <summary>
        /// Goes to the first level and starts listening to the OnEnterNextLevel
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            OnEnterNextLevel();
            CarGameManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        } 
        
        /// <summary>
        /// Adds all new enemies to the enemy pool
        /// </summary>
        private void OnEnterNextLevel()
        {
            _currentEnemies.Clear();
            
            foreach (var groundEnemy in CurrentLevel.groundEnemies)
            {
                if (_groundEnemies.Contains(groundEnemy)) continue;
                _groundEnemies.Add(groundEnemy);
            }
            _currentEnemies.AddRange(CurrentLevel.groundEnemies);
            
            foreach (var flyingEnemy in CurrentLevel.flyingEnemies)
            {
                if (_flyingEnemies.Contains(flyingEnemy)) continue;
                _flyingEnemies.Add(flyingEnemy);
            }
            _currentEnemies.AddRange(CurrentLevel.flyingEnemies);
        }
        
        /// <summary>
        /// Picks a random enemy and checks if it is in the current level.
        /// If it isn't it will pick a new random
        /// </summary>
        /// <param name="spawner"> the spawner of the enemy </param>
        public void CreateEnemy(EnemySpawner spawner)
        {
            var newEnemy = spawner.InAir ? 
                RandomFlyingEnemies:
                RandomGroundEnemies;
            if (!_currentEnemies.Contains(newEnemy))
                newEnemy = spawner.InAir ? 
                    RandomFlyingEnemies:
                    RandomGroundEnemies;
            var enemyPrefab = spawner.CreateEnemy(newEnemy);
            enemyPrefab.transform.parent = transform;
            _enemies.Add(enemyPrefab);
        }

        /// <summary>
        /// creates enemy death effect and tells the enemy to destroy itself
        /// </summary>
        /// <param name="enemy"> the enemy to destroy </param>
        /// <param name="addPoints"></param>
        public void DestroyEnemy(EnemyBehaviour enemy, bool addPoints = true)
        {
            if (addPoints) _enemyPoints += enemy.Worth;
            
            _enemies.Remove(enemy);
            if (onDeathParticles == null) return;
            
            Instantiate(onDeathParticles,enemy.transform.position,Quaternion.identity);
            
            Destroy(enemy.gameObject,0.1f);
            enemy.DestroyRigidbody();
            Destroy(enemy);
        }
    }
}
