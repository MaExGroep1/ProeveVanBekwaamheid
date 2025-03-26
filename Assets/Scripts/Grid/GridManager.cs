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
        
        [Header("Spawn animation")]
        [SerializeField] private float fallTime;
        [SerializeField] private RectTransform gridRect;
        [SerializeField] private float gridRectOffset;
        
        private GridElement[,] _grid;                                       // the grid of grid elements


        private void Start()
        {
            CreateGrid();
            foreach (var gridElement in _grid)
            {
                Debug.Log(gridElement.ToString());
            }
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
                    var newBlock = Instantiate(blockTemplate, _grid[i,j].transform);
                    var waitTime = i * j * 0.01f;
                
                    newBlock.Rect.position = new Vector3(_grid[i,j].transform.position.x,_grid[i,j].transform.position.y + gridRect.rect.height + gridRectOffset ,0);

                    StartCoroutine(WaitToDrop(newBlock.gameObject,waitTime));
                
                    newBlock.Initialize(block);
                
                    _grid[i,j].SetBlock(newBlock);
                }
        }

        private IEnumerator WaitToDrop(GameObject newBlockGameObject,float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            LeanTween.moveLocalY(newBlockGameObject, 0, fallTime).setEase(LeanTweenType.easeOutExpo);
        }
    }
}
