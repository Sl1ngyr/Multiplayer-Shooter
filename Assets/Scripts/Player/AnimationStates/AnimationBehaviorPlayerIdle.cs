using Services;
using UnityEngine;

namespace Player.AnimationStates
{
    public class AnimationBehaviorPlayerIdle : AnimationBehavior
    {
        public AnimationBehaviorPlayerIdle(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(Constants.PLAYER_IDLE, true);
        }

        public override void Exit()
        {
            Animator.SetBool(Constants.PLAYER_IDLE, false);
        }
    }
}