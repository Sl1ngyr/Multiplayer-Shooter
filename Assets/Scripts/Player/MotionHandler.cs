using CameraComponents;
using Fusion;
using Services.Network;
using UnityEngine;

namespace Player
{
    public class MotionHandler : NetworkBehaviour
    {
        [SerializeField] private float _speed;
        
        private Rigidbody2D _rigidbody2D;
        
        public override void Spawned()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            
            if (Object.HasInputAuthority)
            {
                Camera camera = Camera.main;
                
                camera.GetComponent<FollowCamera>().CameraAnchorPoint = transform;
            }
        }

        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);
            if (data.Direction.magnitude > 0)
            {
                data.Direction.Normalize();
                _rigidbody2D.MovePosition(transform.position + Runner.DeltaTime * _speed * (Vector3)data.Direction);
            }

            RPC_ChangeLocalScale(data.Direction.x);
        }

        [Rpc]
        private void RPC_ChangeLocalScale(float directionX)
        {
            if (directionX > 0)
            {
                _rigidbody2D.transform.localScale = new Vector2(1, 1);
            }
            if (directionX < 0)
            {
                _rigidbody2D.transform.localScale = new Vector2(-1, 1);
            }
        }
        
    }
}