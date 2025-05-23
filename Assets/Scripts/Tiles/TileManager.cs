using System;
using CarGame;
using UnityEngine;

namespace Tiles
{
    public class TileManager : Util.Singleton<TileManager>
    {
        [SerializeField] private Transform tileParent;                                      // the parent of the tiles
        [SerializeField] private int levelLength;                                           // the amount of tiles in a level before going to the next
        [SerializeField]private Tile startTile;                                             // the tile at the start of the game

        private Tile _currentTile;                                                          // the tile under the player
        private Tile _previousTile;                                                         // the tile before the current tile
        private int _currentTileIndex;                                                      // the current tile index in the level
        
        public int TileAmount { get; private set; }                                         // the amount of tiles that have passed
        private static LevelData CurrentLevel => CarGameManager.Instance.CurrentLevel;      // the level data of the current level
        
        private Action _onEnterNextLevel;                                                   // event when entering new level

        /// <summary>
        /// Creates start tiles
        /// </summary>
        private void Start()
        {
            CreateNewTile(startTile);
            CreateNewTile(CurrentLevel.RandomTile);
            _currentTileIndex++;
        }
        
        /// <summary>
        /// Adds function to the onMatch event
        /// </summary>
        /// <param name="onEnterNextLevel"> The function to add </param>
        public void ListenToOnEnterNextLevel(Action onEnterNextLevel) => _onEnterNextLevel += onEnterNextLevel;

        /// <summary>
        /// Places a new tile at the end of the previous
        /// </summary>
        private void GenerateNewTile()
        {
            TileAmount++;
            
            _currentTile.transform.parent = tileParent;
            Destroy(_previousTile.gameObject);
            
            if (_currentTileIndex == 0)
            {
                _currentTileIndex++;
                CreateNewTile(CurrentLevel.startTile);
                return;
            }

            if (_currentTileIndex == levelLength)
            {
                GenerateEndTile();
                return;
            }
            
            _currentTileIndex++;
            CreateNewTile(CurrentLevel.RandomTile);
        }
        
        /// <summary>
        /// Generates the end tile of the level
        /// </summary>
        private void GenerateEndTile()
        {
            _currentTileIndex = 0;
            CreateNewTile(CurrentLevel.endTile);
            _onEnterNextLevel?.Invoke();
        }
        
        /// <summary>
        /// Creates a new tile and attaches it to the previous one
        /// </summary>
        /// <param name="tile"> The new tile data </param>
        private void CreateNewTile(Tile tile)
        {
            var attach = tileParent;
            if (_currentTile != null)
            {
                _currentTile.OnTileLoaded -= GenerateNewTile;
                _previousTile = _currentTile;
                attach = _currentTile.TileEnd;
            }
            _currentTile = Instantiate(tile,tileParent);
            _currentTile.transform.position = attach.transform.position;
            _currentTile.OnTileLoaded += GenerateNewTile;
        }
    }
}
