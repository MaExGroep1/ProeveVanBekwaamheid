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

        public RectTransform Rect => rect;              // getter of the rect of the grid element

        private BlockType _blockType;                   // the block type of the block
        
        private Vector3 _gridPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            var parent = transform.parent;
            transform.SetParent(parent.parent);
            transform.SetParent(parent);
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            transform.position = _gridPosition;
        }

        public void Initialize(BlockTypeData data)
        {
            _blockType = data.blockTypes;
            image.sprite = data.blockSprite;
        }
        
        /// <summary>
        /// Gets the block type of the block
        /// </summary>
        /// <returns> the current block type </returns>
        public BlockType GetBlockType() => _blockType;
        
        public void SetPosition(Vector3 position) => _gridPosition = position;
    }
}
