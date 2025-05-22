using System.Collections;
using Grid;
using UnityEngine;

namespace Blocks
{
    public class BombBlock : Block
    {
        protected override void TryToMatch()
        {
            var direction = transform.position - _gridPosition;
            var normalized = direction.normalized;

            GridManager.Instance.HandleBombBlockMatch(_cords, CalculateDirection(normalized), this);
        }

        public override IEnumerator DestroyBlock(float waitTime, float moveTime, float scale)
        {
            LeanTween.scale(gameObject, Vector3.one * (scale * 2), moveTime * 2)
                .setOnComplete(() => LeanTween.scale(gameObject, Vector3.zero, moveTime * 2));

            yield return new WaitForSeconds(moveTime * 3);
            
            Destroy(gameObject);
        }
    }
}
