using System;
using UnityEngine;

namespace Testing
{
    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour
    {
        public float speed;
        
        private Rigidbody _rigidBody;

        private void Awake() => _rigidBody = GetComponent<Rigidbody>();

        void Update()
        { 
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            
            _rigidBody.AddForce(new Vector3(hor, ver).normalized * speed * _rigidBody.mass);
        }
    }
}
