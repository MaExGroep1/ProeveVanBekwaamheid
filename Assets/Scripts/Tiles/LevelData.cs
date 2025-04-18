using UnityEngine;

namespace Tiles
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public Tile startTile;                                          // the start tile of the level
        public Tile[] tiles;                                            // array of available tiles of the level
        public Tile endTile;                                            // the end tile of the level
        public Tile RandomTile => tiles[Random.Range(0,tiles.Length)];  // a random tile from "tiles"
    }
}
