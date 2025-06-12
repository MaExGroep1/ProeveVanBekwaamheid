using UnityEngine;

namespace Car
{
    public class FollowCarDynamicY : MonoBehaviour
    {
        [SerializeField] private Transform car;
        [SerializeField] private float ySpeed;
        private void Update()
        {
            var distance = Mathf.Abs(transform.position.y - car.position.y);
            var position = new Vector2(car.position.x, transform.position.y);

            transform.position = position;
            transform.position = Vector2.MoveTowards(
                transform.position,
                car.position,
                ySpeed * distance * Time.deltaTime);
        }
    }
}
