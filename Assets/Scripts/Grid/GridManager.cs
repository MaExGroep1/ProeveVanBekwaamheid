using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

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
        [SerializeField] private BlockData[] blockData;                     // the available block types
        [SerializeField] private float blockPlaceDistance;                  // the distance the block can travel before swapping 
        [SerializeField] private float blockSpringBackDistance;             // the distance the block can travel before not snapping back to its origin
        
        [Header("Block times")]
        [SerializeField] private float blockTravelTime;                     // the time it takes for the block to travel somewhere
        [SerializeField] private float blockFallTime;                       // the time it takes for the block to fall somewhere
        
        [Header("Block destroy")]
        [SerializeField] private float blockWaitTime;                       // the time it takes for a new block to be made after a match is made
        [SerializeField] private float blockTravelSpeed;                    // The speed the block moves to the destroy point
        [SerializeField] private float blockDestroyScale;                   // the scale of the block when traveling to the destroy point
        
        [Header("Spawn animation")]
        [SerializeField] private RectTransform gridRect;                    // the rect of the grid object
        [SerializeField] private float gridRectOffset;                      // the offset to add to the fall
        
        [Header("Shuffle data")]
        [SerializeField] private int maxAttempts = 100;                     // the max amount of attempts a shuffle can try a shuffle combination
        
        private GridElement[,] _grid;                                       // the grid of grid elements
        private Transform _blocksParent;                                    // the parent of all the blocks
        private readonly List<int> _checkedColumns = new();                 // a list of columns that have been checked
        private float _heightOffset;                                        // the vertical distance between grid rows
        
        public float BlockPlaceDistance => blockPlaceDistance;              // the available block types
        public float BlockSpringBackDistance => blockSpringBackDistance;    // the available block types
        public float BlockTravelTime => blockTravelTime;                    // the available block types
        public float BlockFallTime => blockFallTime;                        // the available block types
        public GridElement[,] Grid => _grid;
        
        private Action<BlockType, int> _onMatch;                            // the event to invoke when a match is made
        
        private void Start()
        {
            CreateGrid();
        }
        
        /// <summary>
        /// Adds function to the onMatch event
        /// </summary>
        /// <param name="onMatch"> the function to add </param>
        public void ListenToOnMatch(Action<BlockType, int> onMatch) => _onMatch += onMatch;
        
        /// <summary>
        /// Removes function to the onMatch event
        /// </summary>
        /// <param name="onMatch"> the function to remove </param>
        public void StopListeningToOnMatch(Action<BlockType, int> onMatch) => _onMatch -= onMatch;
        
        /// <summary>
        /// Shuffles all blocks on the grid while ensuring no immediate matches exist.
        /// </summary>
        public void Shuffle()
        {
            var allBlocks = new List<Block>();
            var validShuffle = false;
            var attempts = 0;
            
            foreach (var element in _grid)
            {
                if (element.GetBlock() == null) continue;
                allBlocks.Add(element.GetBlock());
                element.SetBlock(null);
            }

            while (!validShuffle && attempts < maxAttempts)
            {
                var index = 0;
                attempts++;

                for (var i = allBlocks.Count - 1; i > 0; i--)
                {
                    var randomIndex = Random.Range(0, i + 1);
                    (allBlocks[i], allBlocks[randomIndex]) = (allBlocks[randomIndex], allBlocks[i]);
                }

                foreach (var element in _grid)
                {
                    element.SetBlock(allBlocks[index]);
                    index++;
                }

                validShuffle = !HasThreeInARow();
            }

            blockTravelTime *= 5f;
            foreach (var element in _grid)
                element.GetBlock()?.GoToOrigin(null);   
            blockTravelTime *= 0.2f;
        }

        /// <summary>
        /// Checks if the grid contains any three or more matching blocks in a row or column.
        /// </summary>
        private bool HasThreeInARow()
        {
            var rows = _grid.GetLength(0);
            var cols = _grid.GetLength(1);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols - 2; j++)
                {
                    var a = _grid[i, j].GetBlock();
                    var b = _grid[i, j + 1].GetBlock();
                    var c = _grid[i, j + 2].GetBlock();

                    if (a != null && b != null && c != null &&
                        a.GetBlockType() == b.GetBlockType() && 
                        b.GetBlockType() == c.GetBlockType())
                        return true;
                }
    

            for (int i = 0; i < cols; i++)
                for (int j = 0; j < rows - 2; j++)
                {
                    var a = _grid[j, i].GetBlock();
                    var b = _grid[j + 1, i].GetBlock();
                    var c = _grid[j + 2, i].GetBlock();

                    if (a != null && b != null && c != null &&
                        a.GetBlockType() == b.GetBlockType() && 
                        b.GetBlockType() == c.GetBlockType())
                        return true;
                }
            return false;
        }
        
        /// <summary>
        /// Generates new blocks in a specified column to replace missing ones
        /// </summary>
        /// <param name="y"> the index where new blocks should be generated </param>
        public void GenerateNewBlocks(int y)
        {
            if (_checkedColumns.Contains(y)) return;
            var blocks = new List<Block>();
            var emptyElements = 0;

            _checkedColumns.Add(y);
            for (int i = 0; i < gridHeight ; i++)
            {
                if (_grid[i, y].GetBlock() == null)
                    emptyElements++;
            }
            for (int i = 0; i < emptyElements; i++)
            {
                var newBlock = Instantiate(blockTemplate, _blocksParent);
                var block = GetRandomBlock();
                var position = new Vector2(_grid[gridHeight-1,y].transform.position.x, _grid[gridHeight-1,y].transform.position.y);
                var offset = new Vector2(0, _heightOffset * (i + 1));
                
                newBlock.Initialize(block.blockType,block.destroyDestination.position);
                newBlock.Rect.position = position + offset;
                blocks.Add(newBlock);
            }
            var reverse = blocks.ToArray();
            Array.Reverse(reverse);
            FallBlocks(y,reverse);
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

            if (!IsWithinBounds(index))
            {
                _grid[cords.x, cords.y].GetBlock().GoToOrigin(null);
                return;
            }
            
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
        /// Attempts to match a block after it has fallen into position
        /// </summary>
        /// <param name="cords"> the coordinates of the block </param>
        /// <param name="blockType"> the type of the block </param>
        private void TryMatchFromFall(Vector2Int cords , BlockType blockType)
        {
            
            int vertical = 1 + CheckDirection(cords, new Vector2Int(1, 0), blockType);

            int horizontal = 1 + CheckDirection(cords, new Vector2Int(0, 1), blockType)
                               + CheckDirection(cords, new Vector2Int(0, -1), blockType);

            if (vertical <= 2 && horizontal <= 2) return;
            StartCoroutine(DestroyMatchingBlocks(cords, blockType ,vertical > 2, horizontal > 2));
        }
        
        /// <summary>
        /// Causes blocks to fall into empty spaces within a column
        /// </summary>
        /// <param name="y"> the column index </param>
        /// <param name="extraBlocks"> the new blocks to be placed at the top </param>
        private void FallBlocks(int y , Block[] extraBlocks)
        {
            for (int i = 0; i < gridHeight; i++)
            {
                if (_grid[i,y].GetBlock() != null) continue;
                for (int j = i; j < gridHeight; j++)
                {
                    if (_grid[j,y].GetBlock() == null) continue;
                    _grid[i,y].SetBlock(_grid[j, y].GetBlock());
                    _grid[j,y].SetBlock(null);
                    break;
                }
            }

            for (int i = 0; i < extraBlocks.Length; i++)
                _grid[gridHeight-i-1,y].SetBlock(extraBlocks[i]);

            for (int i = 0; i < gridHeight; i++)
            {
                if (_grid[i,y].GetBlock() == null) return;
                var i1 = i;
                _grid[i,y].GetBlock().FallToOrigin(()=> 
                    TryMatchFromFall(new Vector2Int(i1,y),_grid[i1,y].GetBlock().GetBlockType())
                );
            }
            _checkedColumns.Clear();
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
            
            newElement.SetCords(x, y);
            
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

            _heightOffset = _grid[1, 0].transform.position.y - _grid[0, 0].transform.position.y;
        }
        
        /// <summary>
        /// Adds blocks to all grid elements
        /// </summary>
        private void PopulateGrid()
        {
            for (int i = 0; i < gridHeight; i++)
                for (int j = 0; j < gridWidth; j++)
                {
                    var exclusions = new List<BlockType>();
                    var newBlock = Instantiate(blockTemplate, _blocksParent);
                    var waitTime = (i + 1) * 0.05f + (j + 1) * 0.05f;
                    var position = new Vector2(_grid[i,j].transform.position.x, _grid[i,j].transform.position.y);
                    var offset = new Vector2(0,gridRect.rect.height + gridRectOffset);
                    if (i > 1 && _grid[i - 1, j].GetBlockType() == _grid[i - 2, j].GetBlockType())
                        exclusions.Add(_grid[i - 1, j].GetBlockType());
                    if (j > 1 && _grid[i ,j - 1].GetBlockType() == _grid[i ,j - 2].GetBlockType())
                        exclusions.Add(_grid[i, j - 1].GetBlockType());

                    var block = GetRandomBlocksExcluding(exclusions.ToArray());

                    newBlock.Rect.position = position + offset;
                    
                    newBlock.Initialize(block.blockType,block.destroyDestination.position);
                
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
            StartCoroutine(DestroyMatchingBlocks(cordsA, _grid[cordsA.x, cordsA.y].GetBlock().GetBlockType(), horizontalA, verticalA));
            StartCoroutine(DestroyMatchingBlocks(cordsB, _grid[cordsB.x, cordsB.y].GetBlock().GetBlockType(), horizontalB, verticalB));
        }
        
        /// <summary>
        /// Destroys all adjacent blocks of the same type
        /// </summary>
        /// <param name="cords"> the cords to delete from </param>
        /// <param name="blockType"> the type of block </param>
        /// <param name="horizontal"> whether to delete on horizontal </param>
        /// <param name="vertical"> whether to delete on vertical </param>
        private IEnumerator DestroyMatchingBlocks(Vector2Int cords, BlockType blockType, bool horizontal, bool vertical)
        {
            var hor = 0;
            var ver = 0;
            if (horizontal)
            {
                hor += DestroyBlocksFromDirection(cords, new Vector2Int( 1, 0), blockType);
                hor += DestroyBlocksFromDirection(cords, new Vector2Int(-1, 0), blockType);
            }
            if (vertical)
            {
                ver += DestroyBlocksFromDirection(cords, new Vector2Int( 0, 1), blockType);
                ver += DestroyBlocksFromDirection(cords, new Vector2Int( 0,-1), blockType);
            }

            if (!vertical && !horizontal) yield break;
            yield return StartCoroutine(_grid[cords.x, cords.y].GetBlock().DestroyBlock(blockWaitTime,blockTravelSpeed,blockDestroyScale));
            _onMatch?.Invoke(blockType, hor+ver+1);
        }
        
        /// <summary>
        /// Destroys all blocks of 1 type in a direction from a point
        /// </summary>
        /// <param name="cords"> the cord where to delete from </param>
        /// <param name="direction"> the direction to delete </param>
        /// <param name="blockType"> the type to delete </param>
        private int DestroyBlocksFromDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            int i = 1;
            while (IsWithinBounds(cords + i * direction) && _grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType)
            {
                StartCoroutine(_grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlock().DestroyBlock(blockWaitTime,blockTravelSpeed,blockDestroyScale));
                i++;
            }

            return i - 1;
        }
        
        /// <summary>
        /// Gets a random block from the block table
        /// </summary>
        /// <returns> A random block type </returns>
        private BlockData GetRandomBlock() => blockData[Random.Range(0, blockData.Length)];

        /// <summary>
        /// Gets a random block from the block table
        /// </summary>
        /// <returns> A random block type </returns>
        private BlockData GetRandomBlocksExcluding(BlockType[] excluding)
        {
            var blockTypes = blockData.ToList();
            foreach (var blockType in blockTypes.ToList().Where(blockType => excluding.Contains(blockType.blockType.blockTypes)))
            {
                blockTypes.Remove(blockType);
            }

            return blockTypes[Random.Range(0, blockTypes.Count)];
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
            newBlock.FallToOrigin(null);
        }
    }
}