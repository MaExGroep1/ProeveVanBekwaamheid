using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Blocks;
using Car;
using Sound;
using UnityEngine;
using Random = UnityEngine.Random;
using Util;

namespace Grid
{
    public class GridManager : Singleton<GridManager>
    {
        [Header("Audio")] 
        [SerializeField] private SoundService bombSound;                    // the sound of the bomb match
        
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
        [SerializeField] private float blockTravelSpeed;                    // the speed the block moves to the destroy point
        [SerializeField] private float blockDestroyScale;                   // the scale of the block when traveling to the destroy point

        [Header("Bomb")]
        [SerializeField] private int bombBlockRange;                        // the range of the bomb block when it explodes
        [SerializeField] private Transform bombBlockDestroyDestination;     // the range of the bomb block when it explodes
        [SerializeField] private BombBlock bombBlockTemplate;               // the template of the bomb block 
        [SerializeField] private int bombBlockSpawnRate;                    // the chance the bomb block can spawn when not on the board
        [SerializeField] private CarShockWave carShockWave;                 //
        
        [Header("Spawn animation")]
        [SerializeField] private RectTransform gridRect;                    // the rect of the grid object
        [SerializeField] private float gridRectOffset;                      // the offset to add to the fall
        
        [Header("Shuffle data")]
        [SerializeField] private int maxAttempts = 100;                     // the max amount of attempts a shuffle can try a shuffle combination
        [SerializeField] private float shuffleDuration;                     // the time it takes for the grid to shuffle

        private Transform _blocksParent;                                    // the parent of all the blocks
        private readonly List<int> _checkedColumns = new();                 // a list of columns that have been checked
        private float _heightOffset;                                        // the vertical distance between grid rows
        private bool _isBombOnGrid = true;                                         // the bool to check if there's a bomb block on the grid
        private bool _hasMatched;                                           // whether the play has had a match
        
        public float BlockPlaceDistance => blockPlaceDistance;              // the available block types
        public float BlockSpringBackDistance => blockSpringBackDistance;    // the available block types
        public float BlockTravelTime => blockTravelTime;                    // the available block types
        public float BlockFallTime => blockFallTime;                        // the available block types
        public GridElement[,] Grid { get; private set; }                    // the grid of grid elements

        private Action<BlockType, int> _onMatch;                            // the event to invoke when a match is made
        private Action _onFirstMatch;                                       // the event to invoke when a match is made
        
        private void Start() => CreateGrid();
        
        /// <summary>
        /// Adds function to the onMatch event
        /// </summary>
        /// <param name="onMatch"> The function to add </param>
        public void ListenToOnMatch(Action<BlockType, int> onMatch) => _onMatch += onMatch;
        
        /// <summary>
        /// Removes function to the onMatch event
        /// </summary>
        /// <param name="onFirstMatch"> The function to add </param>
        public void ListenToOnFirstMatch(Action onFirstMatch) => _onFirstMatch += onFirstMatch;
        
        /// <summary>
        /// Shuffles all blocks on the grid while ensuring no immediate matches exist.
        /// </summary>
        public void Shuffle()
        {
            var allBlocks = new List<Block>();
            var validShuffle = false;
            var attempts = 0;
            
            foreach (var element in Grid)
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

                foreach (var element in Grid)
                {
                    element.SetBlock(allBlocks[index]);
                    index++;
                }

                validShuffle = !HasThreeInARow();
            }

            blockTravelTime *= 5f;
            foreach (var element in Grid)
                element.GetBlock()?.GoToOrigin(null,shuffleDuration);   
            blockTravelTime *= 0.2f;
        }

        /// <summary>
        /// Checks if the grid contains any three or more matching blocks in a row or column.
        /// </summary>
        private bool HasThreeInARow()
        {
            var rows = Grid.GetLength(0);
            var cols = Grid.GetLength(1);

            for (var x = 0; x < rows; x++)
                for (var y = 0; y < cols - 2; y++)
                {
                    var a = Grid[x, y].GetBlock();
                    var b = Grid[x, y + 1].GetBlock();
                    var c = Grid[x, y + 2].GetBlock();

                    if (a != null && b != null && c != null &&
                        a.GetBlockType() == b.GetBlockType() && 
                        b.GetBlockType() == c.GetBlockType())
                        return true;
                }
    

            for (var x = 0; x < cols; x++)
                for (var y = 0; y < rows - 2; y++)
                {
                    var a = Grid[y, x].GetBlock();
                    var b = Grid[y + 1, x].GetBlock();
                    var c = Grid[y + 2, x].GetBlock();

                    if (a != null && b != null && c != null &&
                        a.GetBlockType() == b.GetBlockType() && 
                        b.GetBlockType() == c.GetBlockType())
                        return true;
                }
            return false;
        }

        private bool HasBombInGrid() =>
            Grid.Cast<GridElement>().Any(element => element.GetBlockType() == BlockType.Null);
        
        
        /// <summary>
        /// Generates new blocks in a specified column to replace missing ones
        /// </summary>
        /// <param name="y"> The index where new blocks should be generated </param>
        public void GenerateNewBlocks(int y)
        {
            _isBombOnGrid = HasBombInGrid();
            if (_checkedColumns.Contains(y)) return;
            var blocks = new List<Block>();
            var emptyElements = 0;

            _checkedColumns.Add(y);
            
            for (var i = 0; i < gridHeight ; i++)
            {
                if (Grid[i, y].GetBlock() == null)
                    emptyElements++;
            }
            
            for (var i = 0; i < emptyElements; i++)
            {
                var isBomb = Random.Range(0, bombBlockSpawnRate) == 1 && !_isBombOnGrid;
                var newBlock = isBomb
                    ? Instantiate(bombBlockTemplate, _blocksParent)
                    : Instantiate(blockTemplate, _blocksParent);

                var block = GetRandomBlock();
                
                var position = new Vector2(Grid[gridHeight-1,y].transform.position.x, Grid[gridHeight-1,y].transform.position.y);
                var offset = new Vector2(0, _heightOffset * (i + 1));

                if (isBomb)
                {
                    _isBombOnGrid = true;
                    newBlock.GetComponent<BombBlock>().SetDestroyDestination(bombBlockDestroyDestination);
                }
                else
                    newBlock.Initialize(block.blockType,block.destroyDestination);
                
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
        /// <param name="cords"> The cords of the block </param>
        /// <param name="direction"> The direction the block is going to </param>
        /// <param name="blockType"> The type of the block </param>
        public void TryMatch(Vector2Int cords, Direction direction, BlockType blockType)
        {
            var otherDir = direction switch
            {
                Direction.Up    => Direction.Down,
                Direction.Down  => Direction.Up,
                Direction.Left  => Direction.Right,
                Direction.Right => Direction.Left,
                _               => direction 
            };

            var index = cords + DirectionToCords(direction);

            if (!IsWithinBounds(index))
            {
                Grid[cords.x, cords.y].GetBlock().GoToOrigin(null);
                return;
            }
            
            var otherType = Grid[index.x, index.y].GetBlockType();
            var horizontalA = 1 + (direction == Direction.Down  ? 0 : CheckDirection(index, new Vector2Int(1, 0), blockType)) 
                                + (direction == Direction.Up    ? 0 : CheckDirection(index, new Vector2Int(-1, 0), blockType));

            var verticalA = 1 + (direction == Direction.Right ? 0 : CheckDirection(index, new Vector2Int(0, -1), blockType)) 
                              + (direction == Direction.Left  ? 0 : CheckDirection(index, new Vector2Int(0, 1), blockType));
            
            var horizontalB = 1 + (otherDir == Direction.Down  ? 0 : CheckDirection(cords, new Vector2Int(1, 0), otherType)) 
                                + (otherDir == Direction.Up    ? 0 : CheckDirection(cords, new Vector2Int(-1, 0), otherType));

            var verticalB = 1 + (otherDir == Direction.Right ? 0 : CheckDirection(cords, new Vector2Int(0, -1), otherType)) 
                              + (otherDir == Direction.Left  ? 0 : CheckDirection(cords, new Vector2Int(0, 1), otherType));


            if (horizontalA <= 2 && verticalA <= 2 && horizontalB <= 2 && verticalB <= 2)
            {
                Grid[cords.x, cords.y].GetBlock().GoToOrigin(null);
                return;
            }

            SwapBlocks(cords, index, () =>
            {
                DestroyAllMatchingBlocks(
                    index, horizontalA > 2, verticalA > 2,
                    cords, horizontalB > 2, verticalB > 2);
            });
        }
        
        /// <summary>
        /// Handles the bomb-block match
        /// </summary>
        /// <param name="originalBombCords"> Reference to the original bomb location </param>
        /// <param name="direction"> The Direction the bomb </param>
        /// <param name="thisBomb"> The bomb block </param>
        public void HandleBombBlockMatch(Vector2Int originalBombCords, Direction direction, BombBlock thisBomb)
        {
            var bombCords = originalBombCords + DirectionToCords(direction);
                        
            if (!IsWithinBounds(bombCords))
            {
                thisBomb.GoToOrigin(null);
                return;
            }
            var otherBlock = Grid[bombCords.x, bombCords.y].GetBlock();
            
            Grid[bombCords.x,bombCords.y].SetBlock(thisBomb);
            Grid[originalBombCords.x,originalBombCords.y].SetBlock(otherBlock);
            
            thisBomb.GoToOrigin(null);
            
            otherBlock.GoToOrigin(() => HandleBombBlockExplosion(bombCords));
        }
        
        /// <summary>
        /// Converts a direction into a Vector2Int
        /// </summary>
        /// <param name="direction"> A direction </param>
        /// <returns> The direction in cords</returns>
        private static Vector2Int DirectionToCords(Direction direction) =>
            direction switch
            {
                Direction.Up    => new Vector2Int( 1,  0),
                Direction.Down  => new Vector2Int(-1,  0),
                Direction.Left  => new Vector2Int( 0, -1),
                Direction.Right => new Vector2Int( 0,  1),
                _               => Vector2Int.zero
            };

        /// <summary>
        /// Attempts to match a block after it has fallen into position
        /// </summary>
        /// <param name="cords"> The coordinates of the block </param>
        /// <param name="blockType"> The type of the block </param>
        private void TryMatchFromFall(Vector2Int cords , BlockType blockType)
        {
            
            var vertical = 1 + CheckDirection(cords, new Vector2Int(1, 0), blockType);

            var horizontal = 1 + CheckDirection(cords, new Vector2Int(0, 1), blockType)
                               + CheckDirection(cords, new Vector2Int(0, -1), blockType);

            if (vertical <= 2 && horizontal <= 2) return;
            StartCoroutine(DestroyMatchingBlocks(cords, blockType ,vertical > 2, horizontal > 2));
        }
        
        /// <summary>
        /// Causes blocks to fall into empty spaces within a column
        /// </summary>
        /// <param name="y"> The column index </param>
        /// <param name="extraBlocks"> The new blocks to be placed at the top </param>
        private void FallBlocks(int y , Block[] extraBlocks)
        {
            for (var x = 0; x < gridHeight; x++)
            {
                if (Grid[x,y].GetBlock() != null) continue;
                for (var x2 = x; x2 < gridHeight; x2++)
                {
                    if (Grid[x2,y].GetBlock() == null) continue;
                    Grid[x,y].SetBlock(Grid[x2, y].GetBlock());
                    Grid[x2,y].SetBlock(null);
                    break;
                }
            }

            for (var i = 0; i < extraBlocks.Length; i++)
                Grid[gridHeight-i-1,y].SetBlock(extraBlocks[i]);

            for (var i = 0; i < gridHeight; i++)
            {
                if (Grid[i,y].GetBlock() == null) return;
                var i1 = i;
                Grid[i,y].GetBlock().FallToOrigin(()=> 
                    TryMatchFromFall(new Vector2Int(i1,y),Grid[i1,y].GetBlock().GetBlockType())
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
            for (var x = 0; x < gridHeight; x++)
                for (var y = 0; y < gridWidth; y++)
                    grid = CreateGridElement(x,y, grid);

            
            Grid = grid;
            _blocksParent = Grid[gridHeight-1,gridWidth-1].transform;
            
            AlignGrid();
            PopulateGrid();
        }
        
        /// <summary>
        /// Creates a grid elements and adds it to a 2d array
        /// </summary>
        /// <param name="x"> The x cord </param>
        /// <param name="y"> The y cord </param>
        /// <param name="grid"> The grid to add the grid element to</param>
        /// <returns> The original grid with the new element added </returns>
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
            for (var x = 0; x < gridHeight; x++)
            {
                var heightStart = unitPadding * (x + 0.5f) / 100 + heightScale * x;
                var heightEnd = heightStart + heightScale;
                for (var y = 0; y < gridWidth; y++)
                {
                    var widthStart = unitPadding * (y + 0.5f) / 100 + widthScale * y;
                    var widthEnd = widthStart + widthScale;
                    Grid[x,y].Rect.anchorMax = new Vector2(widthEnd, heightEnd);
                    Grid[x,y].Rect.anchorMin = new Vector2(widthStart, heightStart);
                }
            }

            _heightOffset = Grid[1, 0].transform.position.y - Grid[0, 0].transform.position.y;
        }
        
        /// <summary>
        /// Adds blocks to all grid elements
        /// </summary>
        private void PopulateGrid()
        {
            var bombCords = new Vector2Int(Random.Range(0, gridHeight),Random.Range(0, gridWidth));
            
            for (var x = 0; x < gridHeight; x++)
            {
                for (var y = 0; y < gridWidth; y++)
                {
                    var isBomb = bombCords == new Vector2Int(x, y);
                    
                    var exclusions = new List<BlockType>();
                    
                    var newBlock = isBomb
                        ? Instantiate(bombBlockTemplate, _blocksParent)
                        : Instantiate(blockTemplate,_blocksParent);
                    
                    if (x > 1 && Grid[x - 1, y].GetBlockType() == Grid[x - 2, y].GetBlockType())
                        exclusions.Add(Grid[x - 1, y].GetBlockType());
                    if (y > 1 && Grid[x, y - 1].GetBlockType() == Grid[x, y - 2].GetBlockType())
                        exclusions.Add(Grid[x, y - 1].GetBlockType());

                    var block = GetRandomBlocksExcluding(exclusions.ToArray());

                    newBlock.Rect.position = CalculateRectPosition(Grid[x, y]);

                    if (isBomb)
                        newBlock.GetComponent<BombBlock>().SetDestroyDestination(bombBlockDestroyDestination);
                    else
                        newBlock.Initialize(block.blockType, block.destroyDestination);


                    Grid[x, y].SetBlock(newBlock);

                    StartCoroutine(WaitToDrop(newBlock, CalculateWaitTime(x, y)));
                }
            }
        }
        
        /// <summary>
        /// Calculates the wait time for WaitToFall
        /// </summary>
        /// <param name="cordX"> The X coordinates of the falling block </param>
        /// <param name="cordY"> The Y coordinates of the falling block </param>
        /// <returns> Returns a float of the time </returns>
        private static float CalculateWaitTime(int cordX, int cordY) =>
            (cordX + 1) * 0.05f + (cordY + 1) * 0.05f;
        
        /// <summary>
        /// Checks how many blocks are the same type
        /// </summary>
        /// <param name="cords"> The cords to check from </param>
        /// <param name="direction"> The direction to check </param>
        /// <param name="blockType"> The type the current block is </param>
        /// <returns> The amount of blocks of the same type in a row </returns>
        private int CheckDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            var i = 1;
            while (IsWithinBounds(cords + i * direction) && Grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType) i++;
            return i - 1;
        }
        
        /// <summary>
        /// Checks if the cord is in the bound of the grid
        /// </summary>
        /// <param name="cords"> The cords to check from </param>
        /// <returns> If the cord is in the bound of the grid </returns>
        private bool IsWithinBounds(Vector2Int cords) => cords.x >= 0 && cords.x < gridHeight && cords.y >= 0 && cords.y < gridWidth;
        
        /// <summary>
        /// Swaps two blocks in the grid 
        /// </summary>
        /// <param name="blockAIndex"> The first block </param>
        /// <param name="blockBIndex"> The second block </param>
        /// <param name="onComplete"> When the blocks are done moving </param>
        private void SwapBlocks(Vector2Int blockAIndex, Vector2Int blockBIndex, Action onComplete)
        {
            var blockAElement = Grid[blockAIndex.x, blockAIndex.y];
            var blockBElement = Grid[blockBIndex.x, blockBIndex.y];
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
        /// <param name="cordsA"> The cords of block A </param>
        /// <param name="horizontalA"> Whether to delete on A horizontal </param>
        /// <param name="verticalA"> Whether to delete on A vertical </param>
        /// <param name="cordsB"> The cords of block B </param>
        /// <param name="horizontalB"> Whether to delete on A horizontal </param>
        /// <param name="verticalB"> Whether to delete on A vertical </param>
        private void DestroyAllMatchingBlocks(Vector2Int cordsA, bool horizontalA, bool verticalA,Vector2Int cordsB , bool horizontalB, bool verticalB )
        {
            StartCoroutine(DestroyMatchingBlocks(cordsA, Grid[cordsA.x, cordsA.y].GetBlock().GetBlockType(), horizontalA, verticalA));
            StartCoroutine(DestroyMatchingBlocks(cordsB, Grid[cordsB.x, cordsB.y].GetBlock().GetBlockType(), horizontalB, verticalB));
        }
        
        /// <summary>
        /// Destroys all adjacent blocks of the same type
        /// </summary>
        /// <param name="cords"> The cords to delete from </param>
        /// <param name="blockType"> The type of block </param>
        /// <param name="horizontal"> Whether to delete on horizontal </param>
        /// <param name="vertical"> Whether to delete on vertical </param>
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
            yield return 
                StartCoroutine(
                    Grid[cords.x, cords.y]
                    .GetBlock()
                    .DestroyBlock(
                        blockWaitTime,
                        blockTravelSpeed, 
                        blockDestroyScale)
                    );
            var total = hor + ver + 1;
            _onMatch?.Invoke(blockType, total * total / 3);
            if (_hasMatched) yield break;
            _onFirstMatch?.Invoke();
            _hasMatched = true;
        }

        /// <summary>
        /// Destroys all blocks of 1 type in a direction from a point
        /// </summary>
        /// <param name="cords"> The cord where to delete from </param>
        /// <param name="direction"> The direction to delete </param>
        /// <param name="blockType"> The type to delete </param>
        private int DestroyBlocksFromDirection(Vector2Int cords, Vector2Int direction, BlockType blockType)
        {
            var i = 1;
            while (IsWithinBounds(cords + i * direction) && Grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlockType() == blockType)
            {
                StartCoroutine(Grid[cords.x + i * direction.x, cords.y + i * direction.y].GetBlock().DestroyBlock(blockWaitTime,blockTravelSpeed,blockDestroyScale));
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
            
            foreach (var blockType in blockTypes.ToList())
                if (excluding.Contains(blockType.blockType.blockTypes)) 
                    blockTypes.Remove(blockType);
            
            return blockTypes[Random.Range(0, blockTypes.Count)];
        }
        
        /// <summary>
        /// Handles the logic of the bomb explosion
        /// </summary>
        /// <param name="bombCords"> The Coordinates of the place the explosion should originate </param>
        private void HandleBombBlockExplosion(Vector2Int bombCords)
        {
            var total = 0;
            for (var x = -bombBlockRange; x <= bombBlockRange; x++)
                for (var y = -bombBlockRange; y <= bombBlockRange; y++)
                {
                    var targetCords = new Vector2Int(bombCords.x + x, bombCords.y + y);
                    
                    if (!IsWithinBounds(targetCords)) continue;
                    
                    var block = Grid[targetCords.x, targetCords.y].GetBlock();

                    total++;
                    _isBombOnGrid = false;
                    StartCoroutine(targetCords == bombCords
                        ? block.DestroyBlock(blockWaitTime, blockTravelSpeed, blockDestroyScale,carShockWave.Shockwave)
                        : block.DestroyBlockBomb(blockWaitTime, blockTravelTime, blockDestroyScale));
                }
            bombSound.PlaySound();
            _onMatch?.Invoke(BlockType.Null, total / 2);
            if (_hasMatched) return;
            _onFirstMatch?.Invoke();
            _hasMatched = true;

        }
        
        /// <summary>
        /// Calculates the position of the Rect
        /// </summary>
        /// <param name="grid"> Reference to the location on the grid </param>
        /// <returns> Returns the Rect location </returns>
        private Vector2 CalculateRectPosition(GridElement grid)
        {
            var position = new Vector2(grid.transform.position.x, grid.transform.position.y);
            var offset = new Vector2(0, gridRect.rect.height + gridRectOffset);

            return position + offset;
        }
        
        /// <summary>
        /// Waits for a set amount of time then moves the block to the position
        /// </summary>
        /// <param name="newBlock"> The block to move</param>
        /// <param name="waitTime"> The time to wait before falling </param>
        /// <returns></returns>
        private static IEnumerator WaitToDrop(Block newBlock, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            newBlock.FallToOrigin(null);
        }
    }
}