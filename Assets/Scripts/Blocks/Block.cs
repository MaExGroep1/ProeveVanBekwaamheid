using Grid;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Blocks
{
    public class Block : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private Image image;           // the image of the block
        [SerializeField] private RectTransform rect;    // the rect of the grid element
        
        private BlockType _blockType;                   // the block type of the block
        
        private Vector3 _gridPosition;                  // the default position of the block
        private int _x;
        private int _y;
        
        private float _placeDistance;                   // the available block types
        private float _springBackDistance;              // the available block types

        
        public RectTransform Rect => rect;              // getter of the rect of the grid element

        public void OnBeginDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            transform.SetParent(parent.parent);
            transform.SetParent(parent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
            if (Vector3.Distance(transform.position, _gridPosition) > _placeDistance)
            {
                
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _gridPosition;
        }

        /// <summary>
        /// Sets the block type and sprite
        /// </summary>
        /// <param name="data"> the block type data </param>
        /// <param name="placeDistance"></param>
        /// <param name="springBackDistance"></param>
        public void Initialize(BlockTypeData data, float placeDistance, float springBackDistance)
        {
            _blockType = data.blockTypes;
            image.sprite = data.blockSprite;
            _placeDistance = placeDistance;
            _springBackDistance = springBackDistance;
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
    }
}
