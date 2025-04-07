using System;
using System.Collections;
using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Screen = UnityEngine.Screen;

namespace Blocks
{
    public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;           // the image of the block
        [SerializeField] private RectTransform rect;    // the rect of the grid element
        
        private BlockType _blockType;                   // the block type of the block
        
        private Vector3 _gridPosition;                  // the default position of the block
        private Vector2Int _cords;                      // the cords in the grid

        private bool _isMoving;                         // whether the block is moving
        private bool _canMoveWithMouse;                 // whether the block can stick to the mouse
        
        public RectTransform Rect => rect;              // getter of the rect of the grid element
        
        /// <summary>
        /// Sets the block type and sprite
        /// </summary>
        /// <param name="data"> the block type data </param>
        public void Initialize(BlockTypeData data)
        {
            _blockType = data.blockTypes;
            image.sprite = data.blockSprite;
        }
        
        /// <summary>
        /// Sets the cords of this block
        /// </summary>
        /// <param name="cords"> the cords of the block</param>
        public void SetCords(Vector2Int cords) => _cords = cords;
        
        /// <summary>
        /// Gets the block type of the block
        /// </summary>
        /// <returns> the current block type </returns>
        public BlockType GetBlockType() => _blockType;
        
        /// <summary>
        /// Sets the default position of the block
        /// </summary>
        /// <param name="position"> the origin of the block </param>
        public void SetPosition(Vector3 position) => _gridPosition = position;
        
        /// <summary>
        /// Makes the block go to its origin point
        /// </summary>
        /// <param name="onComplete"> when the block gets to its origin</param>
        public void GoToOrigin(Action onComplete)
        {
            if (LeanTween.isTweening(gameObject)) LeanTween.cancel(gameObject);

            _canMoveWithMouse = false;
            _isMoving = true;
            LeanTween.move(gameObject, _gridPosition, GridManager.Instance.BlockTravelTime).
                setEase(LeanTweenType.easeInCubic).
                setOnComplete(()=>StopMoving(onComplete));
        }

        /// <summary>
        /// Makes the block fall to its origin point
        /// </summary>
        public void FallToOrigin(Action onComplete)
        {
            if (LeanTween.isTweening(gameObject)) LeanTween.cancel(gameObject);
            
            _canMoveWithMouse = false;
            _isMoving = true;
            LeanTween.moveY(gameObject, _gridPosition.y, GridManager.Instance.BlockFallTime).
                setEase(LeanTweenType.easeInCubic).
                setOnComplete(()=>StopMoving(onComplete));
        }
        
        /// <summary>
        /// Sets the block to movable and sets the block to the front
        /// </summary>
        /// <param name="eventData"> Mouse data </param>
        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_isMoving) return;
            var parent = transform.parent;
            _canMoveWithMouse = true;
            transform.SetParent(parent.parent);
            transform.SetParent(parent);
        }
        
        /// <summary>
        /// Checks if it can move then moves it to the mouse
        /// </summary>
        /// <param name="eventData"> Mouse data </param>
        public void OnDrag(PointerEventData eventData)
        {
            if(_isMoving || !_canMoveWithMouse) return;
            transform.position = Input.mousePosition;
            if (Vector3.Distance(transform.position, _gridPosition) > GridManager.Instance.BlockPlaceDistance * Screen.height / 1920)
                TryToMatch();
        }
        
        /// <summary>
        /// Checks if it can match
        /// </summary>
        /// <param name="eventData"> Mouse data </param>
        public void OnEndDrag(PointerEventData eventData)
        {
            if(_isMoving) return;
            if (Vector3.Distance(transform.position, _gridPosition) > GridManager.Instance.BlockSpringBackDistance * Screen.height / 1920)
            {
                TryToMatch();
                return;
            }

            GoToOrigin(null);
        }
        
        /// <summary>
        /// Starts the matching process
        /// </summary>
        private void TryToMatch()
        {
            var direction =  transform.position - _gridPosition;
            var normalized = direction.normalized;
            var dir = Mathf.Abs(normalized.x) < Mathf.Abs(normalized.y) ?
                normalized.y > 0 ? 
                    Direction.Up: 
                    Direction.Down: 
                normalized.x > 0 ? 
                    Direction.Right: 
                    Direction.Left;

            GridManager.Instance.TryMatch(_cords, dir, _blockType);
        }
        
        /// <summary>
        /// Sets the block to immobile 
        /// </summary>
        /// <param name="onComplete"> invokes the stop movement</param>
        private void StopMoving(Action onComplete)
        {
            _isMoving = false;
            onComplete?.Invoke();
        }
        
        /// <summary>
        /// Destroys the block after a certain time
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator DestroyBlock(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
            yield return 0;
            GridManager.Instance.GenerateNewBlocks(_cords.y);
        }
    }
}
