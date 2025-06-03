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
        
        private readonly List<EnemyBehaviour> _groundEnemies = new();                                           // list of available ground enemies
        private readonly List<EnemyBehaviour> _flyingEnemies = new();                                           // list of available flying enemies

        private static LevelData CurrentLevel => CarGameManager.Instance.CurrentLevel;                          // the level data of the current level
        private EnemyBehaviour RandomGroundEnemies => _groundEnemies[Random.Range(0, _groundEnemies.Count-1)];    // a random ground enemy
        private EnemyBehaviour RandomFlyingEnemies => _flyingEnemies[Random.Range(0, _flyingEnemies.Count-1)];    // a random flying enemy

        [field: SerializeField] public float EnemyMultiplierAmount { get; private set; }                        // the amount of tiles to have instantiated before making the difficulty plus 1
        public List<EnemyBehaviour> Enemies { get; private set; } = new();                                      // a list of all active enemies
        public float EnemyPoints { get; private set; }                                                          // the amount of points the user has gotten from enemies

        
        /// <summary>
        /// Goes to the first level and starts listening to the OnEnterNextLevel
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            OnEnterNextLevel(false);
            CarGameManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        } 
        
        /// <summary>
        /// Adds all new enemies to the enemy pool
        /// </summary>
        private void OnEnterNextLevel(bool hasLooped)
        {
            if (hasLooped) return;
            
            foreach (var groundEnemy in CurrentLevel.groundEnemies)
                _groundEnemies.Add(groundEnemy);
            
            foreach (var flyingEnemy in CurrentLevel.flyingEnemies)
                _flyingEnemies.Add(flyingEnemy);
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
            var enemyPrefab = spawner.CreateEnemy(newEnemy);
            enemyPrefab.transform.parent = transform;
            Enemies.Add(enemyPrefab);
        }

        /// <summary>
        /// Creates enemy death effect and tells the enemy to destroy itself
        /// </summary>
        /// <param name="enemy"> the enemy to destroy </param>
        /// <param name="addPoints"> whether to add points to the player </param>
        public void DestroyEnemy(EnemyBehaviour enemy, bool addPoints = true)
        {
            if (addPoints) EnemyPoints += enemy.Worth;
            
            Enemies.Remove(enemy);
            if (onDeathParticles == null) return;
            
            Instantiate(onDeathParticles,enemy.transform.position,Quaternion.identity);
            
            Destroy(enemy.gameObject,0.1f);
            enemy.DestroyRigidbody();
            Destroy(enemy);
        }
    }
}
