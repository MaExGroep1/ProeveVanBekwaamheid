using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(fileName = "BlockTypeTableData", menuName = "ScriptableObjects/BlockTypeTableData", order = 1)]
    public class BlockTypeTableData : ScriptableObject
    {
        public BlockTypeData[] blockTypes;  // all types of block in circulation
        
        /// <summary>
        /// Gets a random block from the block table
        /// </summary>
        /// <returns> A random block type </returns>
        public BlockTypeData GetRandomBlock() => blockTypes[Random.Range(0, blockTypes.Length)];
    }
}
