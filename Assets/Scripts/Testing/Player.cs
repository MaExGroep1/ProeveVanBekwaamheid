using UnityEngine;

namespace Testing
{
    public class Player : MonoBehaviour
    {
        public float speed;
        void Update()
        { 
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            
            transform.position += new Vector3(hor, ver, 0) * Time.deltaTime * speed;
        }
    }
}
