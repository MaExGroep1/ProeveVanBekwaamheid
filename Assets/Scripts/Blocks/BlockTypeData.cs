using UnityEngine;

namespace Blocks
{
    [CreateAssetMenu(fileName = "BlockTypeData", menuName = "ScriptableObjects/BlockTypeData", order = 1)]
    public class BlockTypeData : ScriptableObject
    {
        public BlockType blockTypes;        // the block type of this data
        public Sprite blockSprite;          // the icon of this block type
    }
}
