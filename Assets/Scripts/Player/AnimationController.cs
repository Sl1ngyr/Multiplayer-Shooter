using Fusion;
using Player.AnimationStates;
using Services;
using Services.Network;
using UnityEngine;

namespace Player
{
    public class AnimationController : NetworkBehaviour
    {
        private Animator _animator;
        private AnimationBehavior _animationBehavior;
        private PlayerHealthSystem _playerHealthSystem;
        
        private bool _isPlayerDeath = false;
        
        public override void Spawned()
        {
            _animator = GetComponent<Animator>();
            _playerHealthSystem = GetComponent<PlayerHealthSystem>();
            
            _animationBehavior = new AnimationBehaviorPlayerIdle(_animator);
            _animationBehavior.Enter();

            _playerHealthSystem.OnPlayerDead += PlayerDeath;
        }

        public override void FixedUpdateNetwork()
        {
            if(_isPlayerDeath) return;
            
            var input = GetInput(out NetworkInputData data);

            if (data.Direction.magnitude > 0)
            {
                _animationBehavior.Exit();
                _animationBehavior = new AnimationBehaviorPlayerRun(_animator);
                _animationBehavior.Enter();
            }
            else
            {
                _animationBehavior.Exit();
                _animationBehavior = new AnimationBehaviorPlayerIdle(_animator);
                _animationBehavior.Enter();
            }
        }

        public void PlayerDeath()
        {
            _isPlayerDeath = true;
            _animationBehavior.Exit();
            _animationBehavior = new AnimationBehaviorPlayerDeath(_animator);
            _animationBehavior.Enter();
        }
        
        private void OnDisable()
        {
            _playerHealthSystem.OnPlayerDead -= PlayerDeath;
        }
        
    }
}