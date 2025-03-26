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
                    
                    newBlock.SetPosition(position);

                    StartCoroutine(WaitToDrop(newBlock.gameObject, position.y, waitTime));
                
                    newBlock.Initialize(block, new Vector2Int(i,j), blockPlaceDistance, blockSpringBackDistance);
                
                    _grid[i,j].SetBlock(newBlock);
                }
        }
        
        /// <summary>
        /// Waits for a set amount of time then moves the block to the position
        /// </summary>
        /// <param name="newBlockGameObject"> the block to move</param>
        /// <param name="to"> the place to move it to </param>
        /// <param name="waitTime"> the time to wait before falling </param>
        /// <returns></returns>
        private IEnumerator WaitToDrop(GameObject newBlockGameObject, float to, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            LeanTween.moveY(newBlockGameObject, to, fallTime).setEase(LeanTweenType.easeInCubic);
        }

        private int CheckUp(Vector2Int cords, BlockType blockType)
        {
            var i = 1;
            while (cords.x + i <= gridHeight && _grid[cords.x + i, cords.y].GetBlockType() == blockType) i++;
            return i-1;
        }
        
        private int CheckDown(Vector2Int cords, BlockType blockType)
        {
            var i = 1;
            while (cords.x - i >= 0 && _grid[cords.x - i, cords.y].GetBlockType() == blockType) i++;
            return i-1;
        }
        private int CheckRight(Vector2Int cords, BlockType blockType)
        {
            var i = 1;
            while (cords.y + i <= gridWidth && _grid[cords.x, cords.y + i].GetBlockType() == blockType) i++;
            return i-1;
        }
        
        private int CheckLeft(Vector2Int cords, BlockType blockType)
        {
            var i = 1;
            while (cords.y - i >= 0 && _grid[cords.x, cords.y - i].GetBlockType() == blockType) i++;
            return i-1;
        }


        public void TryMatch(Vector2Int cords ,Direction direction ,BlockType blockType)
        {
            var offset = direction switch
            {
                Direction.Up    => new Vector2Int( 1, 0),
                Direction.Down  => new Vector2Int(-1, 0),
                Direction.Left  => new Vector2Int( 0,-1),
                Direction.Right => new Vector2Int( 0, 1),
                _               => new Vector2Int( 0, 0)
            };
            var index = offset + cords;
            int hor = 1;
            int ver = 1;
            hor += direction == Direction.Down ? 0 : CheckUp(index, blockType);
            hor += direction == Direction.Up ? 0 :CheckDown(index, blockType);
            ver += direction == Direction.Right ? 0 :CheckLeft(index, blockType);
            ver += direction == Direction.Left ? 0 :CheckRight(index, blockType);
            if (hor > 2 || ver > 2)
            {
                
            }
        }

        private void SwapBlocks(Vector2Int blockA, Vector2Int blockB)
        {
            
        }
    }
}
