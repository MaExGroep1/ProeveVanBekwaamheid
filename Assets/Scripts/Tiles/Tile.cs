using System;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        [SerializeField] private Transform tileEnd;     // the end of this tile to spawn the next tile
        [SerializeField] private TileLoader loadNext;   // the loader of the tile to load the next tile
        [SerializeField] private Collider backWall;     // the wall at the back of the tile
        
        public Action OnTileLoaded                      // the event to invoke when loading a new tile
        {
            get => loadNext._onLoad;
            set => loadNext._onLoad = value;
        }
    
        public Transform TileEnd => tileEnd;
        
        /// <summary>
        /// Turns on the back wall
        /// </summary>
        public void EnableBackWall() => backWall.enabled = true;
        
        /// <summary>
        /// Draws a dot on the start point of the tile
        /// </summary>
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1,1,0,0.5f);
            Gizmos.DrawSphere(transform.position, 1f);
            Gizmos.color = new Color(0,0,0);
            Gizmos.DrawSphere(transform.position, 0.1f);

        }
    }
}
