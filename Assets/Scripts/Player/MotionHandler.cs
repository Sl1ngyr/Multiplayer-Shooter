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
        private PlayerHealthSystem _playerHealthSystem;
        private CapsuleCollider2D _capsuleCollider2D;

        private bool _isPlayerDead = false;

        public bool IsPlayerDead => _isPlayerDead;
        
        public override void Spawned()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
            _playerHealthSystem = GetComponent<PlayerHealthSystem>();
            
            if (Object.HasInputAuthority)
            {
                Camera camera = Camera.main;
                
                camera.GetComponent<FollowCamera>().CameraAnchorPoint = transform;
            }

            _playerHealthSystem.OnPlayerDead += DeactivateComponents;
        }

        public override void FixedUpdateNetwork()
        {
            if(_isPlayerDead) return;
            
            var input = GetInput(out NetworkInputData data);
            
            if (data.Direction.magnitude > 0)
            {
                data.Direction.Normalize();
                _rigidbody2D.MovePosition(transform.position + Runner.DeltaTime * _speed * (Vector3)data.Direction);
            }

            RPC_ChangeLocalScale(data.Direction.x);
        }

        private void DeactivateComponents()
        {
            if(_isPlayerDead) return;
            
            _capsuleCollider2D.enabled = false;
            _rigidbody2D.simulated = false;

            _isPlayerDead = true;
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
        
        private void OnDisable()
        {
            _playerHealthSystem.OnPlayerDead -= DeactivateComponents;
        }
        
    }
}