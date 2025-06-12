using UnityEngine;

namespace Util
{
    public class TransformToScreenSpace : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset;

        private void Update() => transform.position = Camera.main!.WorldToScreenPoint(target.position) + offset;
    
    }
}
