using System.Linq;
using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(fileName = "BlockTypeTableData", menuName = "ScriptableObjects/BlockTypeTableData", order = 1)]
    public class BlockTypeTableData : ScriptableObject
    {
        public BlockTypeData[] allBlockTypes;  // all types of block in circulation
        
        /// <summary>
        /// Gets a random block from the block table
        /// </summary>
        /// <returns> A random block type </returns>
        public BlockTypeData GetRandomBlock() => allBlockTypes[Random.Range(0, allBlockTypes.Length)];

        /// <summary>
        /// Gets a random block from the block table
        /// </summary>
        /// <returns> A random block type </returns>
        public BlockTypeData GetRandomBlocksExcluding(BlockType[] excluding)
        {
            var blockTypes = allBlockTypes.ToList();
            foreach (var blockType in blockTypes.ToList().Where(blockType => excluding.Contains(blockType.blockTypes)))
            {
                blockTypes.Remove(blockType);
            }

            return blockTypes[Random.Range(0, blockTypes.Count - 1)];
        }

    }
}
