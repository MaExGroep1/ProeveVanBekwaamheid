using System;
using Tiles;
using UnityEngine;
using Util;

namespace CarGame
{
    public class CarGameManager : Singleton<CarGameManager>
    {
        [SerializeField] private LevelData[] levels;                        // all the playable levels
        private int _currentLevelIndex;                                     // the current level of the game
        public LevelData CurrentLevel => levels[_currentLevelIndex];

        private Action _onEnterNextLevel;
        
        private void Start() => TileManager.Instance.ListenToOnEnterNextLevel(OnEnterNextLevel);
        
        /// <summary>
        /// Adds function to the onMatch event
        /// </summary>
        /// <param name="onEnterNextLevel"> the function to add </param>
        public void ListenToOnEnterNextLevel(Action onEnterNextLevel) => _onEnterNextLevel += onEnterNextLevel;
        
        /// <summary>
        /// Removes function to the onMatch event
        /// </summary>
        /// <param name="onEnterNextLevel"> the function to remove </param>
        public void StopListeningToOnEnterNextLevel(Action onEnterNextLevel) => _onEnterNextLevel -= onEnterNextLevel;

        
        private void OnEnterNextLevel()
        {
            if (_currentLevelIndex < levels.Length - 1)
            {
                _currentLevelIndex++;
                return;
            }
            _currentLevelIndex = 0;
        }
    }
}
