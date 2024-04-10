using Fusion;
using Player.AnimationStates;
using Services.Network;
using UnityEngine;

namespace Player
{
    public class AnimationController : NetworkBehaviour
    {
        private Animator _animator;

        private AnimationBehavior _animationBehavior;

        private bool _isPlayerDeath = false;
        
        public override void Spawned()
        {
            _animator = GetComponent<Animator>();
            _animationBehavior = new AnimationBehaviorPlayerIdle(_animator);
            _animationBehavior.Enter();
        }

        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);

            if (data.Direction.magnitude > 0 && !_isPlayerDeath)
            {
                _animationBehavior.Exit();
                _animationBehavior = new AnimationBehaviorPlayerRun(_animator);
                _animationBehavior.Enter();
            }
            else if(!_isPlayerDeath)
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
    }
}