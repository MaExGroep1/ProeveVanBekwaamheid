using Grid;
using UnityEngine;

namespace Blocks
{
    public class BombBlock : Block
    {
        private Vector3 _gridPosition;
        private Vector2Int _cords;
        
        protected override void TryToMatch()
        {
            var direction =  transform.position - _gridPosition;
            var normalized = direction.normalized;

            GridManager.Instance.HandleBombBlockMatch(_cords, CalculateDirection(normalized), this);
        }
    }
}
