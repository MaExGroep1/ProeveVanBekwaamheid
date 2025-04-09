using System;
using System.Collections.Generic;
using Tiles;
using UnityEngine;
using Util;

namespace Enemy
{
    public class EnemyManager : Singleton<EnemyManager>
    {
        [SerializeField] private EnemyBehaviour enemyPrefab;
        private List<EnemyBehaviour> _enemies = new();
        private int _level;

        public EnemyBehaviour EnemyPrefab => enemyPrefab;
        private void Start()
        {
            TileManager.Instance.ListenToOnMatch(OnEnterNextLevel);
        }

        private void OnEnterNextLevel() => _level++;
    }
}
