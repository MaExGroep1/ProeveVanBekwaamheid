using System;
using Car;
using Tiles;
using UnityEngine;
using Util;

namespace CarGame
{
    public class CarGameManager : Singleton<CarGameManager>
    {
        [SerializeField] private LevelData[] levels;                        // all the playable levels
        private int _currentLevelIndex;                                     // the current level of the game
        public LevelData CurrentLevel => levels[_currentLevelIndex];        // the current level of the game

        private bool _hasLooped;
        private Action<bool> _onEnterNextLevel;                             // invokes when it gets to a new level
        
        private void Start() => TileManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        
        /// <summary>
        /// Adds function to the onMatch event
        /// </summary>
        /// <param name="onEnterNextLevel"> the function to add </param>
        public void ListenToOnEnterNextLevel(Action<bool> onEnterNextLevel) => _onEnterNextLevel += onEnterNextLevel;
        
        /// <summary>
        /// Sets the next level to the current level +1 or the first level
        /// </summary>
        private void OnEnterNextLevel()
        {
            _onEnterNextLevel?.Invoke(_hasLooped);

            if (_currentLevelIndex < levels.Length - 1)
            {
                _currentLevelIndex++;
                return;
            }
            _currentLevelIndex = 0;
            _hasLooped = true;
        }
    }
}
