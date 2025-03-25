using Blocks;
using UnityEngine;
using UnityEngine.Serialization;

namespace Grid
{
    public class GridElement : MonoBehaviour
    {
        private Block _currentBlock;                    // the current block on the grid element
        
        [SerializeField] private RectTransform rect;    // the rect of the grid element

        public RectTransform Rect => rect;              // getter of the rect of the grid element
        
        /// <summary>
        /// Sets the current block to the "block"
        /// </summary>
        /// <param name="block"> the new block to set</param>
        public void SetBlock(Block block) => _currentBlock = block;
        
        /// <summary>
        /// Gets the current block type of the block
        /// </summary>
        /// <returns> current block type of the block </returns>
        public BlockType GetBlockType() => _currentBlock.GetBlockType();
        
    }
}
