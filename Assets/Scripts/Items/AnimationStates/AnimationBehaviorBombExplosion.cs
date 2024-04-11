using Services;
using UnityEngine;

namespace Items.AnimationStates
{
    public class AnimationBehaviorBombExplosion : AnimationBehavior
    {
        public AnimationBehaviorBombExplosion(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetTrigger(DescriptionItemAnimation.BOMB_EXPLOSION_TRIGGER);
        }

        public override void Exit()
        {
            Animator.ResetTrigger(DescriptionItemAnimation.BOMB_EXPLOSION_TRIGGER);
        }
    }
}