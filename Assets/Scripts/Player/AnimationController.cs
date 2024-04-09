using Fusion;
using Player.AnimationStates;
using Services;
using UnityEngine;

namespace Player
{
    public class AnimationController : NetworkBehaviour
    {
        private Animator _animator;

        private AnimationBehavior _animationBehavior;


        public override void Spawned()
        {
            _animator = GetComponent<Animator>();
            _animationBehavior = new AnimationBehaviorIdle(_animator);
            _animationBehavior.Enter();
        }

        public override void FixedUpdateNetwork()
        {
            var input = GetInput(out NetworkInputData data);

            if (data.Direction.magnitude > 0)
            {
                _animationBehavior.Exit();
                _animationBehavior = new AnimationBehaviorRun(_animator);
                _animationBehavior.Enter();
            }
            else
            {
                _animationBehavior.Exit();
                _animationBehavior = new AnimationBehaviorIdle(_animator);
                _animationBehavior.Enter();
            }
        }
    }
}