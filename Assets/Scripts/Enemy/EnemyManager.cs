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
        [SerializeField] private ParticleSystem onDeathParticles;
        
        private readonly List<EnemyBehaviour> _groundEnemies = new();
        private readonly List<EnemyBehaviour> _flyingEnemies = new();
        
        private List<EnemyBehaviour> _enemies = new(); 

        private static LevelData CurrentLevel => CarGameManager.Instance.CurrentLevel;      // the level data of the current level
        private EnemyBehaviour RandomGroundEnemies => _groundEnemies[Random.Range(0, _groundEnemies.Count)];
        private EnemyBehaviour RandomFlyingEnemies => _flyingEnemies[Random.Range(0, _flyingEnemies.Count)];

        protected override void Awake()
        {
            base.Awake();
            OnEnterNextLevel();
            CarGameManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        } 

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
                spawner.CreateEnemy(RandomFlyingEnemies):
                spawner.CreateEnemy(RandomGroundEnemies);
            newEnemy.transform.parent = transform;
            _enemies.Add(newEnemy);
        }

        public void DestroyEnemy(EnemyBehaviour enemy)
        {
            _enemies.Remove(enemy);
            if (onDeathParticles == null) return;
            Instantiate(onDeathParticles,enemy.transform.position,Quaternion.identity);
            enemy.DestroySelf(0.1f);
        }
    }
}
