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

        private bool _isMoving;
        
        public RectTransform Rect => rect;              // getter of the rect of the grid element

        public void OnBeginDrag(PointerEventData eventData)
        {
            if(_isMoving) return;
            var parent = transform.parent;
            transform.SetParent(parent.parent);
            transform.SetParent(parent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(_isMoving) return;
            transform.position = Input.mousePosition;
            if (Vector3.Distance(transform.position, _gridPosition) > GridManager.Instance.BlockPlaceDistance * Screen.height / 1920)
                TryToMatch();
            
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if(_isMoving) return;
            if (Vector3.Distance(transform.position, _gridPosition) > GridManager.Instance.BlockSpringBackDistance* Screen.height / 1920)
            {
                TryToMatch();
                return;
            }

            GoToOrigin(null);
        }

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
        /// Sets the block type and sprite
        /// </summary>
        /// <param name="data"> the block type data </param>
        /// <param name="cords"> the cords of the block</param>
        public void Initialize(BlockTypeData data, Vector2Int cords)
        {
            _blockType = data.blockTypes;
            image.sprite = data.blockSprite;
            _cords = cords;
        }
        
        /// <summary>
        /// Gets the block type of the block
        /// </summary>
        /// <returns> the current block type </returns>
        public BlockType GetBlockType() => _blockType;
        
        /// <summary>
        /// Sets the default position of the block
        /// </summary>
        /// <param name="position"></param>
        public void SetPosition(Vector3 position) => _gridPosition = position;

        public void GoToOrigin(Action onComplete)
        {
            _isMoving = true;
            LeanTween.move(gameObject, _gridPosition, GridManager.Instance.BlockSpringBackSpeed).
                setEase(LeanTweenType.easeInCubic).
                setOnComplete(()=>StopMoving(onComplete));
        }

        private void StopMoving(Action onComplete)
        {
            _isMoving = false;
            onComplete?.Invoke();
        }

        public IEnumerator DestroyBlock(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(gameObject);
        }
    }
}
