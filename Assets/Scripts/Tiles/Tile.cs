using System;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Transform tileEnd;     // the end of this tile to spawn the next tile
        [SerializeField] private TileLoader loadNext;   // the loader of the tile to load the next tile
        
        public Action OnTileLoaded                      // the event to invoke when loading a new tile
        {
            get => loadNext.OnLoad;
            set => loadNext.OnLoad = value;
        }
    
        public Transform TileEnd => tileEnd;
        
        /// <summary>
        /// Draws a dot on the start point of the tile
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
}
