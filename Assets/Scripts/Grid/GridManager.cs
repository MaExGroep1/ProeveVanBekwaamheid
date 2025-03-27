using System;
using System.Collections;
using System.Collections.Generic;
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
        [SerializeField] private float blockPlaceDistance;                  // the available block types
        [SerializeField] private float blockSpringBackDistance;             // the available block types
        [SerializeField] private float blockSpringBackSpeed;                          // the available block types
        
        [Header("Spawn animation")]
        [SerializeField] private float fallTime;                            // the time it takes to fall to the ground
        [SerializeField] private RectTransform gridRect;                    // the rect of the grid object
        [SerializeField] private float gridRectOffset;                      // the offset to add to the fall
        
        private GridElement[,] _grid;                                       // the grid of grid elements
        private Transform _blocksParent;                                    // the parent of all the blocks
        
        public float BlockPlaceDistance => blockPlaceDistance;              // the available block types
        public float BlockSpringBackDistance => blockSpringBackDistance;    // the available block types
        public float BlockSpringBackSpeed => blockSpringBackSpeed;          // the available block types



        private void Start()
        {
            CreateGrid();
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
        /// Waits for a set amount of time then moves the block to the position
        /// </summary>
        /// <param name="newBlock"> the block to move</param>
        /// <param name="waitTime"> the time to wait before falling </param>
        /// <returns></returns>
        private IEnumerator WaitToDrop(Block newBlock, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            newBlock.GoToOrigin(null);
        }

        private int CheckDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            int i = 1;
            while (IsWithinBounds(cords + i * direction) && _grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType) i++;
            return i - 1;
        }

        private bool IsWithinBounds(Vector2Int cords)
        {
            return cords.x >= 0 && cords.x < gridHeight && cords.y >= 0 && cords.y < gridWidth;
        }
        
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

            var index = cords + offset;

            int horizontal = 1 + (direction == Direction.Down  ? 0 : CheckDirection(index, new Vector2Int(1, 0), blockType)) 
                               + (direction == Direction.Up    ? 0 : CheckDirection(index, new Vector2Int(-1, 0), blockType));

            int vertical = 1 + (direction == Direction.Right ? 0 : CheckDirection(index, new Vector2Int(0, -1), blockType)) 
                             + (direction == Direction.Left  ? 0 : CheckDirection(index, new Vector2Int(0, 1), blockType));

            if (horizontal <= 2 && vertical <= 2) return;

            SwapBlocks(cords, index, () => DestroyMatchingBlocks(index, blockType, horizontal > 2, vertical > 2));
        }

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

            StartCoroutine(_grid[cords.x, cords.y].GetBlock().DestroyBlock(0.2f));
        }
        
        private void DestroyBlocksFromDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            int i = 1;
            while (IsWithinBounds(cords + i * direction) && _grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType)
            {
                StartCoroutine(_grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlock().DestroyBlock(0.2f));
                i++;
            }
        }

    }
}
