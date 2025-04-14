using System;
using UnityEngine;

namespace Blocks
{
    [Serializable]
    public struct BlockData
    {
        public BlockTypeData blockType;         // the data of the block type
        public Transform destroyDestination;    // the destination of the block when it needs to be destroyed
    }
}