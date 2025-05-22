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
        
        protected Vector3 _gridPosition;                  // the default position of the block
        protected Vector2Int _cords;                      // the cords in the grid
        
        private BlockType _blockType;                   // the block type of the block
        protected Transform _destroyDestination;            // the position of the destroy location of the block
        private bool _isMoving;                         // whether the block is moving
        private bool _canMoveWithMouse;                 // whether the block can stick to the mouse
        
        public RectTransform Rect => rect;              // getter of the rect of the grid element

        /// <summary>
        /// Sets the block type and sprite
        /// </summary>
        /// <param name="data"> the block type data </param>
        /// <param name="destroyDestination"></param>
        public void Initialize(BlockTypeData data, Transform destroyDestination)
        {
            _blockType = data.blockTypes;
            image.sprite = data.blockSprite;
            _destroyDestination = destroyDestination;
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
        /// <param name="duration"> the time it takes for the block to go to its origin </param>
        public void GoToOrigin(Action onComplete, float duration = Mathf.Infinity)
        {
            var time = float.IsPositiveInfinity(duration) ? GridManager.Instance.BlockTravelTime : duration;
            if (LeanTween.isTweening(gameObject)) LeanTween.cancel(gameObject);

            _canMoveWithMouse = false;
            _isMoving = true;
            LeanTween.move(gameObject, _gridPosition, time).
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
        protected virtual void TryToMatch()
        {
            var direction =  transform.position - _gridPosition;
            var normalized = direction.normalized;
            
            
            GridManager.Instance.TryMatch(_cords, CalculateDirection(normalized), _blockType);
        }
        
        protected Direction CalculateDirection(Vector3 normalized)
        {
            return Mathf.Abs(normalized.x) < Mathf.Abs(normalized.y) ?
                normalized.y > 0 ? 
                    Direction.Up: 
                    Direction.Down: 
                normalized.x > 0 ? 
                    Direction.Right: 
                    Direction.Left;
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
        /// <param name="waitTime"> Time to wait before making new blocks</param>
        /// <param name="moveTime"> Time it takes to go to destruction destination</param>
        /// <param name="scale"> The max scale of the blocks while being destroyed</param>
        /// <returns></returns>
        public virtual IEnumerator DestroyBlock(float waitTime, float moveTime, float scale)
        {
            var distance = Vector3.Distance(transform.position, _destroyDestination.position);

            transform.SetParent(transform.parent.parent.parent);
            Destroy(this);
            
            if (LeanTween.isTweening(gameObject))LeanTween.cancel(gameObject);
            
            
            LeanTween.scale(gameObject, Vector3.one * scale , moveTime / 2 * distance).setLoopPingPong();
            LeanTween.moveX(gameObject, _destroyDestination.position.x, moveTime * distance).setEase(LeanTweenType.easeInSine);
            LeanTween.moveY(gameObject, _destroyDestination.position.y, moveTime * distance).setEase(LeanTweenType.easeOutSine).setDestroyOnComplete(gameObject);
            
            yield return new WaitForSeconds(waitTime);
            
            GridManager.Instance.GenerateNewBlocks(_cords.y);
            
            yield return new WaitForSeconds(moveTime * distance - waitTime);
        }
    }
}
