using Grid;
using UnityEngine;

namespace Blocks
{
    public class BombBlock : Block
    {
        private Vector3 _gridPosition;
    
        protected override void TryToMatch()
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

            //GridManager.Instance.HandleBombBlockMatch();
        }
    }
}
