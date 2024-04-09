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
            Animator.SetBool(DescriptionPlayerAnimation.PLAYER_IDLE, true);
        }

        public override void Exit()
        {
            Animator.SetBool(DescriptionPlayerAnimation.PLAYER_IDLE, false);
        }
    }
}