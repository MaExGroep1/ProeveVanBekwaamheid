using UnityEngine;

namespace Tiles
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData", order = 1)]
    public class LevelData : ScriptableObject
    {
        public Tile startTile;
        public Tile[] tiles;
        public Tile endTile;
        
        public Tile RandomTile => tiles[Random.Range(0,tiles.Length)];
    }
}
