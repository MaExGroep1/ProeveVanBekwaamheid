using Blocks;
using UnityEngine;

namespace Grid
{
    public class GridElement : MonoBehaviour
    {
        [SerializeField] private RectTransform rect;    // the rect of the grid element

        private Block _currentBlock;                    // the current block on the grid element
        
        private Vector2Int _cords;                      // the cords in the grid
        
        public RectTransform Rect => rect;              // getter of the rect of the grid element
        
        public void SetCords(int x, int y) => _cords = new Vector2Int(x, y);
        
        /// <summary>
        /// Sets the current block to the "block"
        /// </summary>
        public Block GetBlock() => _currentBlock;

        /// <summary>
        /// Sets the current block to the "block"
        /// </summary>
        /// <param name="block"> the new block to set</param>
        public void SetBlock(Block block)
        {
            _currentBlock = block;
            if (block == null) return;
            block.SetPosition(transform.position);
            block.SetCords(_cords);
        }
        
        /// <summary>
        /// Gets the current block type of the block
        /// </summary>
        /// <returns> current block type of the block </returns>
        public BlockType GetBlockType() => _currentBlock.GetBlockType();
        
    }
}
