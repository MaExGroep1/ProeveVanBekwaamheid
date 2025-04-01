using System;
using System.Collections;
using Blocks;
using UnityEngine;
using Util;

namespace Grid
{
    public class GridManager : Singleton<GridManager>
    {
        [Header("Grid Scale")]
        [SerializeField] private int gridHeight;                            // the amount of columns
        [SerializeField] private int gridWidth;                             // the amount of rows
        
        [Header("Grid padding")]
        [SerializeField] private float unitPadding;                         // the padding between the elements
        
        [Header("Block data")]
        [SerializeField] private GridElement gridElementTemplate;           // the template of the grid element
        [SerializeField] private Block blockTemplate;                       // the template of the block
        [SerializeField] private BlockTypeTableData blockTypeTableData;     // the available block types
        [SerializeField] private float blockPlaceDistance;                  // the distance the block can travel before swapping 
        [SerializeField] private float blockSpringBackDistance;             // the distance the block can travel before not snapping back to its origin
        [SerializeField] private float blockTravelTime;                     // the time it takes for the block to travel somewhere
        [SerializeField] private float blockFallTime;                       // the time it takes for the block to fall somewhere
        
        [Header("Spawn animation")]
        [SerializeField] private RectTransform gridRect;                    // the rect of the grid object
        [SerializeField] private float gridRectOffset;                      // the offset to add to the fall
        
        private GridElement[,] _grid;                                       // the grid of grid elements
        private Transform _blocksParent;                                    // the parent of all the blocks
        
        public float BlockPlaceDistance => blockPlaceDistance;              // the available block types
        public float BlockSpringBackDistance => blockSpringBackDistance;    // the available block types
        public float BlockTravelTime => blockTravelTime;                    // the available block types
        public float BlockFallTime => blockFallTime;                    // the available block types
        
        private void Start()
        {
            CreateGrid();
        }
        
        /// <summary>
        /// Checks if the block has a match
        /// </summary>
        /// <param name="cords"> the cords of the block </param>
        /// <param name="direction"> the direction the block is going to </param>
        /// <param name="blockType"> the type of the block </param>
        public void TryMatch(Vector2Int cords, Direction direction, BlockType blockType)
        {
            var offset = direction switch
            {
                Direction.Up    => new Vector2Int( 1,  0),
                Direction.Down  => new Vector2Int(-1,  0),
                Direction.Left  => new Vector2Int( 0, -1),
                Direction.Right => new Vector2Int( 0,  1),
                _               => Vector2Int.zero
            };
            var otherDir = direction switch
            {
                Direction.Up    => Direction.Down,
                Direction.Down  => Direction.Up,
                Direction.Left  => Direction.Right,
                Direction.Right => Direction.Left,
                _               => direction 
            };

            var index = cords + offset;
            
            var otherType = _grid[index.x, index.y].GetBlockType();
            int horizontalA = 1 + (direction == Direction.Down  ? 0 : CheckDirection(index, new Vector2Int(1, 0), blockType)) 
                               + (direction == Direction.Up    ? 0 : CheckDirection(index, new Vector2Int(-1, 0), blockType));

            int verticalA = 1 + (direction == Direction.Right ? 0 : CheckDirection(index, new Vector2Int(0, -1), blockType)) 
                             + (direction == Direction.Left  ? 0 : CheckDirection(index, new Vector2Int(0, 1), blockType));
            
            int horizontalB = 1 + (otherDir == Direction.Down  ? 0 : CheckDirection(cords, new Vector2Int(1, 0), otherType)) 
                               + (otherDir == Direction.Up    ? 0 : CheckDirection(cords, new Vector2Int(-1, 0), otherType));

            int verticalB = 1 + (otherDir == Direction.Right ? 0 : CheckDirection(cords, new Vector2Int(0, -1), otherType)) 
                             + (otherDir == Direction.Left  ? 0 : CheckDirection(cords, new Vector2Int(0, 1), otherType));


            if (horizontalA <= 2 && verticalA <= 2 && horizontalB <= 2 && verticalB <= 2)
            {
                _grid[cords.x, cords.y].GetBlock().GoToOrigin(null);
                return;
            }
            SwapBlocks(cords, index, () => 
                DestroyAllMatchingBlocks(
                    index, horizontalA > 2, verticalA > 2,
                    cords,horizontalB > 2, verticalB > 2));
        }
        
        /// <summary>
        /// Creates a grid
        /// Then aligns the grid
        /// At lasts populates the grid
        /// </summary>
        private void CreateGrid()
        {
            var grid = new GridElement[gridHeight, gridWidth];
            for (int i = 0; i < gridHeight; i++)
                for (int j = 0; j < gridWidth; j++)
                    grid = CreateGridElement(i,j, grid);
            
            _grid = grid;
            _blocksParent = _grid[gridHeight-1,gridWidth-1].transform;
            
            AlignGrid();
            PopulateGrid();
        }
        
        /// <summary>
        /// Creates a grid elements and adds it to a 2d array
        /// </summary>
        /// <param name="x"> the x cord </param>
        /// <param name="y"> the y cord </param>
        /// <param name="grid"> the grid to add the grid element to</param>
        /// <returns> the original grid with the new element added </returns>
        private GridElement[,] CreateGridElement(int x, int y, GridElement[,] grid)
        {
            var newElement = Instantiate(gridElementTemplate, transform);
            
            grid[x,y] = newElement;
            
            return grid;
        }

        /// <summary>
        /// Calculates all positions of the grid elements
        /// </summary>
        private void AlignGrid()
        {
            var widthScale = (100 - unitPadding * gridWidth) / gridWidth / 100;
            var heightScale = (100 - unitPadding * gridHeight) / gridHeight / 100;
            for (int i = 0; i < gridHeight; i++)
            {
                var heightStart = unitPadding * (i + 0.5f) / 100 + heightScale * i;
                var heightEnd = heightStart + heightScale;
                for (int j = 0; j < gridWidth; j++)
                {
                    var widthStart = unitPadding * (j + 0.5f) / 100 + widthScale * j;
                    var widthEnd = widthStart + widthScale;
                    _grid[i,j].Rect.anchorMax = new Vector2(widthEnd, heightEnd);
                    _grid[i,j].Rect.anchorMin = new Vector2(widthStart, heightStart);
                }
            }
        }
        
        /// <summary>
        /// Adds blocks to all grid elements
        /// </summary>
        private void PopulateGrid()
        {
            for (int i = 0; i < gridHeight; i++)
                for (int j = 0; j < gridWidth; j++)
                {
                    var block = blockTypeTableData.GetRandomBlock();
                    var newBlock = Instantiate(blockTemplate, _blocksParent);
                    var waitTime = i * j * 0.01f;
                    var position = new Vector3(_grid[i,j].transform.position.x, _grid[i,j].transform.position.y,0);
                    var offset = new Vector3(0,gridRect.rect.height + gridRectOffset ,0);
                    newBlock.Rect.position = position + offset;
                    
                    newBlock.Initialize(block, new Vector2Int(i,j));
                
                    _grid[i,j].SetBlock(newBlock);
                    
                    StartCoroutine(WaitToDrop(newBlock, waitTime));
                }
        }
        
        /// <summary>
        /// Checks how many blocks are the same type
        /// </summary>
        /// <param name="cords"> the cords to check from </param>
        /// <param name="direction"> the direction to check </param>
        /// <param name="blockType"> the type the current block is </param>
        /// <returns> the amount of blocks of the same type in a row </returns>
        private int CheckDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            int i = 1;
            while (IsWithinBounds(cords + i * direction) && _grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType) i++;
            return i - 1;
        }
        
        /// <summary>
        /// Checks if the cord is in the bound of the grid
        /// </summary>
        /// <param name="cords"> the cords to check from </param>
        /// <returns> if the cord is in the bound of the grid </returns>
        private bool IsWithinBounds(Vector2Int cords) => cords.x >= 0 && cords.x < gridHeight && cords.y >= 0 && cords.y < gridWidth;
        
        /// <summary>
        /// Swaps two blocks in the grid 
        /// </summary>
        /// <param name="blockAIndex"> the first block </param>
        /// <param name="blockBIndex"> the second block </param>
        /// <param name="onComplete"> when the blocks are done moving </param>
        private void SwapBlocks(Vector2Int blockAIndex, Vector2Int blockBIndex, Action onComplete)
        {
            var blockAElement = _grid[blockAIndex.x, blockAIndex.y];
            var blockBElement = _grid[blockBIndex.x, blockBIndex.y];
            var blockA = blockAElement.GetBlock();
            var blockB = blockBElement.GetBlock();
            
            blockAElement.SetBlock(blockB);
            blockBElement.SetBlock(blockA);
            
            blockA.GoToOrigin(null);
            blockB.GoToOrigin(() => onComplete?.Invoke());
        }
        
        /// <summary>
        /// Destroys all matching blocks near block A and block B
        /// </summary>
        /// <param name="cordsA"> the cords of block A </param>
        /// <param name="horizontalA"> whether to delete on A horizontal </param>
        /// <param name="verticalA"> whether to delete on A vertical </param>
        /// <param name="cordsB"> the cords of block B </param>
        /// <param name="horizontalB"> whether to delete on A horizontal </param>
        /// <param name="verticalB"> whether to delete on A vertical </param>
        private void DestroyAllMatchingBlocks(Vector2Int cordsA, bool horizontalA, bool verticalA,Vector2Int cordsB , bool horizontalB, bool verticalB )
        {
            DestroyMatchingBlocks(cordsA, _grid[cordsA.x, cordsA.y].GetBlock().GetBlockType(), horizontalA, verticalA);
            DestroyMatchingBlocks(cordsB, _grid[cordsB.x, cordsB.y].GetBlock().GetBlockType(), horizontalB, verticalB);
        }
        
        /// <summary>
        /// Destroys all adjacent blocks of the same type
        /// </summary>
        /// <param name="cords"> the cords to delete from </param>
        /// <param name="blockType"> the type of block </param>
        /// <param name="horizontal"> whether to delete on horizontal </param>
        /// <param name="vertical"> whether to delete on vertical </param>
        private void DestroyMatchingBlocks(Vector2Int cords, BlockType blockType, bool horizontal, bool vertical)
        {
            if (horizontal)
            {
                DestroyBlocksFromDirection(cords, new Vector2Int( 1, 0), blockType);
                DestroyBlocksFromDirection(cords, new Vector2Int(-1, 0), blockType);
            }
            if (vertical)
            {
                DestroyBlocksFromDirection(cords, new Vector2Int( 0, 1), blockType);
                DestroyBlocksFromDirection(cords, new Vector2Int( 0,-1), blockType);
            }

            if (vertical || horizontal)
                StartCoroutine(_grid[cords.x, cords.y].GetBlock().DestroyBlock(0.2f));
            
        }
        
        /// <summary>
        /// Destroys all blocks of 1 type in a direction from a point
        /// </summary>
        /// <param name="cords"> the cord where to delete from </param>
        /// <param name="direction"> the direction to delete </param>
        /// <param name="blockType"> the type to delete </param>
        private void DestroyBlocksFromDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            int i = 1;
            while (IsWithinBounds(cords + i * direction) && _grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType)
            {
                StartCoroutine(_grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlock().DestroyBlock(0.2f));
                i++;
            }
        }
        
        /// <summary>
        /// Waits for a set amount of time then moves the block to the position
        /// </summary>
        /// <param name="newBlock"> the block to move</param>
        /// <param name="waitTime"> the time to wait before falling </param>
        /// <returns></returns>
        private IEnumerator WaitToDrop(Block newBlock, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            newBlock.FallToOrigin();
        }
    }
}