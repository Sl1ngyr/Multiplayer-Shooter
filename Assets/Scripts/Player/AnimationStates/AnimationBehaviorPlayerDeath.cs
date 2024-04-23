using Services;
using UnityEngine;

namespace Player.AnimationStates
{
    public class AnimationBehaviorPlayerDeath : AnimationBehavior
    {
        public AnimationBehaviorPlayerDeath(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(Constants.PLAYER_DEATH, true);
        }

        public override void Exit()
        {
            Animator.SetBool(Constants.PLAYER_DEATH, false);
        }
    }
}