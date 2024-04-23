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
            Animator.SetBool(Constants.ENEMY_RUN, true);
        }

        public override void Exit()
        {
            Animator.SetBool(Constants.ENEMY_RUN, false);
        }
    }
}