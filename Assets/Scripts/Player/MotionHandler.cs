using Fusion;
using Services;
using UnityEngine;

namespace Player
{
    public class MotionHandler : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        
        private Rigidbody2D _rigidbody2D;
        
        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);
            if (data.Direction.magnitude > 0)
            {
                data.Direction.Normalize();
                _rigidbody2D.MovePosition(transform.position + Runner.DeltaTime * _speed * (Vector3)data.Direction);
            }
        }
    }
}