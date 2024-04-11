using Player.AnimationStates;
using Services;
using UnityEngine;

namespace Enemy.AnimationStates
{
    public class AnimationBehaviorEnemyRun : AnimationBehavior
    {
        public AnimationBehaviorEnemyRun(Animator animator) : base(animator)
        {
        }

        public override void Enter()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_RUN, true);
        }

        public override void Exit()
        {
            Animator.SetBool(DescriptionEnemyAnimation.ENEMY_RUN, false);
        }
    }
}